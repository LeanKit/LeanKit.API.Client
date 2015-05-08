using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using LeanKit.API.Client.Library;
using LeanKit.API.Client.Library.EventArguments;

namespace LeanKitNotifier
{
	public class LeanKitNotifierApplicationContext : ApplicationContext
	{
		private readonly NotifyIcon _notifyIcon;
		private ILeanKitIntegration _leanKitIntegration;

		public LeanKitNotifierApplicationContext()
		{
			var container = new Container();
			var contextMenu = new ContextMenu();

			//var displaySettings = new MenuItem("Settings");
			//displaySettings.Click += DisplaySettingsOnClick;
			//contextMenu.MenuItems.Add(displaySettings);

			var exitMenu = new MenuItem("Exit");
			exitMenu.Click += ExitMenuOnClick;
			contextMenu.MenuItems.Add(exitMenu);

			// Create the NotifyIcon.
			var iconPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\app.ico";

			_notifyIcon = new NotifyIcon(container)
			{
				Icon = new Icon(iconPath),
				ContextMenu = contextMenu,
				Visible = true,
				Text = @"LeanKit Notifier"
			};

			InitializeLeanKitMonitoring();
		}

		private void InitializeLeanKitMonitoring()
		{
			try
			{
				var host = ConfigurationManager.AppSettings["LeanKit-AccountName"];
				var email = ConfigurationManager.AppSettings["LeanKit-EmailAddress"];
				var password = ConfigurationManager.AppSettings["LeanKit-Password"];
				var boardIds = ConfigurationManager.AppSettings["LeanKit-BoardId"];

				var leanKitAuth = new LeanKitBasicAuth
				{
					Hostname = host,
					Username = email,
					Password = password
				};

				var boardId = int.Parse(boardIds);

				_leanKitIntegration = new LeanKitIntegrationFactory().Create(boardId, leanKitAuth);
				_leanKitIntegration.BoardChanged += IntegrationOnBoardChanged;
				_leanKitIntegration.StartWatching();
			}
			catch (Exception ex)
			{
				ShowMessage("Error: " + ex.Message);
			}
		}

		private void IntegrationOnBoardChanged(object sender, BoardChangedEventArgs boardChangedEventArgs)
		{
			var sb = new StringBuilder();
			if (boardChangedEventArgs.BoardStructureChanged)
				sb.AppendLine("Board structure changed.");

			if (boardChangedEventArgs.AddedCards.Any())
				sb.AppendLine(string.Format("{0} card(s) were added.", boardChangedEventArgs.AddedCards.Count));

			if (boardChangedEventArgs.UpdatedCards.Any())
				sb.AppendLine(string.Format("{0} card(s) were updated.", boardChangedEventArgs.UpdatedCards.Count));

			if (boardChangedEventArgs.MovedCards.Any())
				sb.AppendLine(string.Format("{0} card(s) were moved.", boardChangedEventArgs.MovedCards.Count));

			if (boardChangedEventArgs.BlockedCards.Any())
				sb.AppendLine(string.Format("{0} card(s) were blocked.", boardChangedEventArgs.BlockedCards.Count));

			var message = sb.ToString();
			if (message.Length > 0) ShowMessage(message);
		}

		public void ShowMessage(string message)
		{
			_notifyIcon.BalloonTipText = message;
			_notifyIcon.ShowBalloonTip(1000);
		}

		private void ExitMenuOnClick(object sender, EventArgs eventArgs)
		{
			ExitThreadCore();
		}

		protected override void ExitThreadCore()
		{
			// Signal integration service to stop monitoring
			if (_leanKitIntegration != null) _leanKitIntegration.ShouldContinue = false;

			base.ExitThreadCore();
		}

		private void DisplaySettingsOnClick(object sender, EventArgs eventArgs)
		{
			var form = new SettingsForm(this);
			form.Show();
		}
	}
}
