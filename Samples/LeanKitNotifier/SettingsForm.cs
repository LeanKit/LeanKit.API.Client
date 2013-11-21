using System;
using System.Windows.Forms;

namespace LeanKitNotifier
{
	public partial class SettingsForm : Form
	{
		private readonly LeanKitNotifierApplicationContext _context;

		public SettingsForm(LeanKitNotifierApplicationContext context)
		{
			InitializeComponent();
			_context = context;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ShowMessage("You clicked the button.");
		}

		private void ShowMessage(string message)
		{
			_context.ShowMessage(message);
		}

	}
}
