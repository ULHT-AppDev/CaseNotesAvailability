1. Move your connection strings into this folder. An example of the connection strings config is in here...ConnectionStrings-Example.config.

2. !IMPORTANT - Exclude this folder from GitHub. 					
   To do this navigate to the root folder of the app in file explorer and edit the .GitIgnore file.
   Add in the folder 'ConnectionStrings/' to exclude this folder and all its files from github. You'll know this works as there will be a red stop sign for files in this folder location. 
   Make sure this folder isn't uploading(pushing) to github.

3. Use Version Numbers to let other users know that the file has changed if it changes. i.e.  ConnectionStrings-Example-1.config. This way the web config won't align with a users connection string if outdated.
