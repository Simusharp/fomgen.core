# Fomgen.Core
A .NET library to read/write HLA 1516e FOM

# Prerequesite
.NET Core 5 or later

# How to get
The tool is published as nuget library, you can get it from https://www.nuget.org/packages/Simusharp.FomGen.Core/

# Some code please
The main entry point is class `FomPackage`, it can be used to read, write , or merge Fom modules.

Here is a snippet to read FOM modules:

```C#
// Create a FomPackage instance
var fomPackage = new FomPackageBuilder().Create();

// Read an existing FOM file
using (var stream = new FileStream(_fomPath, FileMode.Open))
{
    fomPackage.ReadFomModule(stream);
}

// You can add a new module
fomPackage.AddFomModule("module1");

// FomPackage class implement IEnumerable<IFomModule>
// Each IFomModule contains properties to set FOM sections (e.g. objects & interactions)

// Validate your package
var validationResult = fomPackage.Validate();

// You can merge all modules into one
var iFomModule = fomPackage.Merge(true); //true: include MIM module

// Write function returns a stream of a zip file that can be saved (e.g. to local or network location)
var stream = fomPackage.Write(true); //true: include MIM module
```

# Contribution
Find an issue? please open an issue to discuss. PRs are welcomed.
