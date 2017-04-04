::
:: This batch file allows us to pass settings to NUnit
:: from VSTS.  Settings are passed on the command line
:: in the format: VstsSettings.bat (key1=value 1) (key2=value 2)
::
:: e.g., VstsSettings.bat (dbUri=https://localhost:8081) (dbKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==)

@echo %* > VstsSettings.txt
