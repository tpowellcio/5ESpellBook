5ESpellBook
===========

Hi everybody!

First off, I would like to thank everyone for the support they have given me for the application and the amount of feedback it has gotten.

This application was written in Visual Studio 2013 using the Microsoft .net stack.  I apologize if you cannot access these tools, however, I have included all files needed to start build and locally run the application if you can support it.

This is the first application I have written and published under git...  If you have suggestions, please let me know!

Minimum requirements:
Database Backend (your choice, I have included a create script for the supporting table)
IIS 7
.net Framework 4.5
MVC 4

Steps:

1) SET UP YOUR DATABASE

	- Use any database back end you want, I used Microsoft Sql Server
	- 
	- Use/Edit the CreateSpells.sql script in the SQLScripts folder to create the spell table.  The USE statement in the script must be filled with your database name.

2) SET UP YOUR WEB.CONFIG

	- Update the connection string in the web.config to specify your new connection string
	- 
	- Update the contributor code in the appsettings area to a contributor code of your liking (this is used for editing/adding spells)

3) BUILD WITH VISUAL STUDIO AND PRAY

	- The project settings are already configured to run off of IIS.  If you meet the minimum requirements on your machine, it should run, as long as you can connect to your database.


Recommended:

	- I know I know... people hate microsoft.  But, I truly believe it is a great tool for publishing applications quickly.  I would totally check out azure services as it can be hosted freely without issue.

LIABILITIES:

I am not responsible for the use or misuse of the code published in this repository.  If you choose to host this application and fill it with copyright materials, you are at your own risk.  I am only responsible for the code hosted at 5espellbook.azurewebsites.net.  If the code is published in any other location, please contact the website owner.

---

I know the code does not have the best algorithms and may be not optimal.  I wrote the code quickly and used it as an exploration of ideas.  Feel free to modify/change/do things with the code as you see fit.  If you have a really cool idea for an update, feel free to send it my way.

If you are new to LINQ or MVC, feel free to use it as tutorial for a basic MVC project.  This code could easily be used as an encounter builder by simply adding in a new entity and appropriate controller to handle it.

Thanks everyone for your love and support,
Philip Vuong
