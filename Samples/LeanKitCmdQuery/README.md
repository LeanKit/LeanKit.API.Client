**LeanKitCmdQuery** is a sample application that demonstrates using the stateless **LeanKitClient** library for making direct API calls. This command-line console application can be used to query and export LeanKit boards, lanes, and cards.

```
Usage: LeanKitCmdQuery options

   OPTION                      TYPE      DESCRIPTION
   -Host(-h)                   string*   LeanKit host name (e.g. CompanyName)
   -User(-u)                   string*   Account email address
   -Password(-p)               string*   Account password
   -Board(-b)                  integer   Specify a board with the given identifier (ID)
   -Boards(-Bo)                switch    List all boards available to account
   -Lanes(-La)                 switch    List all lanes for the given board
   -Lane(-l)                   integer   Specify a lane with the given identifier (ID)
   -IncludeBacklog(-backlog)   switch    Include backlog lane(s)
   -IncludeArchive(-archive)   switch    Include archive lane(s)
   -Cards(-C)                  switch    List all cards for the given board or lane
   -Csv(-Cs)                   switch    Output results in comma-delimited format (CSV)
   -Json(-J)                   switch    Output results in JSON format

   EXAMPLE: LeanKitCmdQuery -h [account name] -u [email] -p [password] [-b [board id]]
   Query LeanKit and output results to console.
```

To export all the cards on a board and save as a comma-delimited (.csv) file:

```
LeanKitCmdQuery -h accountname -u email@address.com -p p@ssw0rd -b 101 -cards -backlog -archive -csv > cards.csv
```