# Banking.cs
Horrible, horrible port of banking.js to C#

Ported some of the stuff out of [Banking.js] (https://github.com/euforic/banking.js/) to C#. 

Basically it'll take your settings call your bank and get the ofx file run it through [OfxSharp] (https://github.com/afonsof/OfxSharp) and give 
you back the OfxSharp.OfxDocument so you can use it.

```csharp
var bank = new Banking(new Ofx.Options{<you options here>});
var ofx = bank.GetStatement();
```
