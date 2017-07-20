# README

### What is MRPlatform?

* C#.NET class library to create custom, standard functionality between SCADA software platforms

### Dependencies

* .NET framework 4.0
* SQL Server/SQL Express 2012 or greater

### Who do I talk to? ###

* Eric Conder, 678-325-2815, econder@mrsystems.com

### Version Information

#### 2.0.4.0
* Added new Hierarchy View to view menu items in a hierarchy by specifying the item's parent record ID.

#### 2.0.3.2
* Modified DeleteNavigationItem parameter to use an integer representing the SQL record Id rather than the screen name when deleting menu items.
* Fixed sort options issue where only custom ascending and descending were available. Now custom, alphabetical ascending, and alphabetical descending are all options using the ItemSortOrder enum in the Menu class.