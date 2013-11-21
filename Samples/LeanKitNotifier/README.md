**LeanKit Notifier** is a sample application that demonstrates using the stateful event subscription **LeanKitClient** library for responding to events raised whenever a LeanKit board changes. This Windows application runs in the tray area and pops up messages whenever cards are updated.

To use this application, open the ```app.config``` and update the LeanKit connection settings.

```
    <add key="LeanKit-AccountName" value="your-account-name" />
    <add key="LeanKit-EmailAddress" value="your-email-address" />
    <add key="LeanKit-Password" value="your-password" />
    <add key="LeanKit-BoardId" value="your-board-id"/>
```
