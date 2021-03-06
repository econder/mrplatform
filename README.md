# README

### What is MRPlatform?

* C#.NET class library to create custom, standard functionality between SCADA software platforms

### Dependencies

* .NET framework 4.0
* SQL Server/SQL Express 2012 or greater

### Who do I talk to?

* Eric Conder, 678-325-2815, econder@mrsystems.com

### When adding/modifying classes...
To prevent COM issues when updating the library, we don't want to automatically generate Guids at compile time. Instead, we assign static, unique Guids to each class, interface, and enum.
* Generate new class and interface Guids if any of the class's method signatures change (e.g. adding/removing/changing method properties and/or method property data types.)
* Each enum gets a unique Guid assigned to it.

### Version Information

#### 2.0.4.0
* Added new Hierarchy View to view menu items in a hierarchy by specifying the item's parent record ID.
* Added Guids to the enums in MRPlatform and Menu classes.

#### 2.0.3.0
* Modified DeleteNavigationItem parameter to use an integer representing the SQL record Id rather than the screen name when deleting menu items.
* Fixed sort options issue where only custom ascending and descending were available. Now custom, alphabetical ascending, and alphabetical descending are all options using the ItemSortOrder enum in the Menu class.
* Fixed an index out of range issue when moving the first or last menu items or or down, respectively.