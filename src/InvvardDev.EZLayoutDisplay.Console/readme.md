# How-to update key definitions
## Javascript definition to JSON
- Open a tab in your prefered browser and go to https://playcode.io/259701?tabs=console&script.js&output
  (or open [KeyDefinitionsJSProcessor.js](https://github.com/Invvard/EZLayoutDisplay/blob/master/resources/Scripts/KeyDefinitionsJSProcessor.js) in an environment where you can run it),
- In an other tab, open the js file : https://configure.ergodox-ez.com/static/js/config/keyDefinitions.js (you may need to use your browser developer tool),
- Select and copy the whole keyCodes constant object (around line 51 to 1243) with the surroundings curly braces,
- Paste it in the KeyDefinitionJSProcessor.js script (replace the previous version),
- Run the script : in the console, you'll find the generated JSON,
- Copy-paste this JSON into a file,
- Save the file under the name 'keyDefinitions.json' in the same folder where you'll run the console APP.

## JSON to EZLayoutDisplay
- Go to the Console App folder,
- Verify the 'keyDefinitions.json' file is present,
- Run the console app,
- If everything went well, you'll see a file named 'keyDefinitions.output.json',

## Integrate the new definitions
- Open Visual Studio,
- In the 'InvvardDev.EZLayoutDisplay.Desktop' project properties, open the file 'keyDefinitions.json',
- In this file, copy-paste the content of the 'keyDefinitions.output.json' you just generated,
- VS should automatically format the pasted JSON but we don't want this, so just CTRL+Z once and it will be inlined back again,
- Save the file.

Now, if you update your layout with the main application, you'll see the characters in their last version.
