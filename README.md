ImaginaryRealities Common Library
=================================
This project contains the source code for the ImaginaryRealities Common Library. The Common Library contains classes and components that we use frequently in the production of our software products, but do not constitute a complete application or product framework. The Common Library contains the following components:

* Semantic version number support

Building the Common Library
---------------------------
In order to build the ImaginaryRealities Common Library, you will need to have the .NET Framework 4.5 installed. The ImaginaryRealities Common Library uses MSBuild to automate the process of building and packaging the common library. In order to make building the Common Library easier, we have create two command scripts that you can use to execute MSBuild for the project. The command scripts are located in the root directory of the project workspace.

* **BUILD.cmd**: This script will execute MSBuild to build the Common Library. The project uses NuGet package restore, so all dependent packages will be downloaded from the NuGet repository.
* **CLEAN.cmd**: This script will execute MSBuild to remove all build-generated files and directories from the project workspace.

License
-------
Copyright &copy; 2013 ImaginaryRealities, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the right to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.