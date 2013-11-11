# ScriptCS Sample

[ScriptCS](http://scriptcs.net/) makes it easy to write and execute C# code. Once you have ScriptCS installed on your PC, all you need is your favorite text editor.

To install ScriptCS, go to [http://scriptcs.net/](http://scriptcs.net/) and follow the instructions.

## ExportCards.csx

This sample scriptcs can be used to export all the cards on a board in comma-delimited (.csv) format. You will need ```ExportCards.csx```, ```CardModel.csx```, and the LeanKit.API.Client.Library assemblies (.dll files). You will also need to modify ```ExportCards.csx``` to provide your LeanKit account name, email address, password, and the Board ID to use.

Once you have set everything up correctly, you can run the following from a command prompt to export cards and save to a local file named ```cards.csv```.

```
scriptcs ExportCards.csx > cards.csv
```