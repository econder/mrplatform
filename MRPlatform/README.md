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

### 2.0.20
* Added AlarmGroup property to MenuItem to allow alarm group functions on the menu buttons on a per-button basis.

### 2.0.19
* Removed rogue WMI.MenuItems class from WMI namespace.
* Added forward/back navigation methods to return last/next navigation history items as MenuItem objects. Also added add & delete history methods.

### 2.0.18
* Bug fixes. Added AssemblyFileVersion attribute.

#### 2.0.11
* Refactored Menu into MenuNavigation. Add MenuFavorites class for menu items favorited by users.

#### 2.0.10
* Bug fixes with menu hierarchy.
* Fix menuOrder issue with orphaned menu items

#### 2.0.7
* Added the ability to get the last parentMenuId to allow navigating backwards in the menu hierarchy.

#### 2.0.6
* Bug fixes.

#### 2.0.5.1
* Added childCount field to a new SQL View called vNavMenu. Menu.GetItemsCollection now queries the vNavMenu view. The childCount field is used by the HMI to display an arrow on the nav button to indicate the menu item has child menu items. The parent menu item's screenName field should probably be ignored by the HMI and used only as a button to show the child navigation items.
* Deleting menu items now has the optional ItemOrphanAction parameter set to either "SetToRoot" or "Delete". SetToRoot will set the parentMenuId of each child item to 0, so the items will display as root menu items. The default action is SetToRoot.

#### 2.0.4.0
* Added new Hierarchy View to view menu items in a hierarchy by specifying the item's parent record ID.
* Added Guids to the enums in MRPlatform and Menu classes.

#### 2.0.3.0
* Modified DeleteNavigationItem parameter to use an integer representing the SQL record Id rather than the screen name when deleting menu items.
* Fixed sort options issue where only custom ascending and descending were available. Now custom, alphabetical ascending, and alphabetical descending are all options using the ItemSortOrder enum in the Menu class.