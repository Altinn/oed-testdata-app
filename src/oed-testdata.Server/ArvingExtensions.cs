using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server;

public static class ArvingExtensions
{
    public static ArvingSkifteattest GetArvingSkifteattestFromPart(Part part)
    {
        return part switch
        {
            ForetakPappPart foretakPappPart => new ForetakPappSkifteattest
            {
                OrganisasjonsNavn = foretakPappPart.OrganisasjonsNavn,
                PaatarGjeldsansvar = false
            },
            ForetakPart foretakPart => new ForetakSkifteattest
            {
                OrganisasjonsNummer = foretakPart.OrganisasjonsNummer!.Value,
                PaatarGjeldsansvar = false
            },
            PersonPappPart personPappPart => new PersonPappSkifteattest
            {
                DateOfBirth = personPappPart.DateOfBirth,
                Navn = personPappPart.Navn,
                PaatarGjeldsansvar = false
            },
            PersonPart personPart => new PersonSkifteattest
            {
                Nin = personPart.Nin,
                PaatarGjeldsansvar = personPart.Formuesfullmakt
            },
            _ => throw new ArgumentOutOfRangeException(nameof(part))
        };
    }
}