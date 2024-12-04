// See https://aka.ms/new-console-template for more information

using oed_testdata.JsonGenerator.Models.Da;
using oed_testdata.JsonGenerator.Models.Tenor;

var cliArgs = Environment.GetCommandLineArgs();
if (cliArgs.Length is < 2 or > 3)
{
    Console.WriteLine($"Usage: {Path.GetFileName(cliArgs[0])} <tenor-file.json> [(optional)output-path]");
    return -1;
}

var tenorFile = cliArgs[1];
try
{
    var tenorPerson = await TenorPersonFile.ValidateAndDeserialize(tenorFile);
    var daFile = DaFile.New(tenorPerson.id, Guid.NewGuid());

    var daCase = daFile.DaCaseList.Single();
    daCase.Parter = tenorPerson.tenorRelasjoner.freg
        .Select(freg =>
            new Parter
            {
                Nin = freg.id,
                Role = freg.tenorRelasjonsnavn.ToUpper(),
                Formuesfullmakt = true,
                PaatarGjeldsansvar = false,
                GodkjennerSkifteattest = false
            })
        .ToArray();

    await DaFile.SerializeAndWrite(daFile, tenorPerson.visningnavn);
}
catch (ArgumentException ae)
{
    Console.WriteLine(ae.Message);
    return -1;
}

return 0;
