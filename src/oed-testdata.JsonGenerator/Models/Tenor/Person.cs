using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oed_testdata.JsonGenerator.Models.Tenor;


public class Person
{
    public Tenorrelasjoner tenorRelasjoner { get; set; }
    public Tenormetadata1 tenorMetadata { get; set; }
    public string id { get; set; }
    public string kjoenn { get; set; }
    public string personstatus { get; set; }
    public string foedeland { get; set; }
    public string foedselsdato { get; set; }
    public string doedsdato { get; set; }
    public string[] identifikator { get; set; }
    public string[] identifikatorType { get; set; }
    public object[] legitimasjonsdokument { get; set; }
    public bool falskIdentitet { get; set; }
    public object[] utenlandskPersonidentifikasjon { get; set; }
    public Hendelsermedsekven1[] hendelserMedSekvens { get; set; }
    public string[] hendelser { get; set; }
    public string sisteHendelse { get; set; }
    public string visningnavn { get; set; }
    public string fornavn { get; set; }
    public string etternavn { get; set; }
    public int navnLengde { get; set; }
    public bool flereFornavn { get; set; }
    public string partnerFnr { get; set; }
    public object[] morFnr { get; set; }
    public object[] farFnr { get; set; }
    public string[] barnFnr { get; set; }
    public int antallBarn { get; set; }
    public bool harRelatertPersonUtenFolkeregisteridentifikator { get; set; }
    public bool harDoedfoedtBarn { get; set; }
    public string[] minRolleForPerson { get; set; }
    public string[] relatertPersonsRolle { get; set; }
    public bool harForeldreMedSammeKjoenn { get; set; }
    public bool harRettsligHandleevne { get; set; }
    public string sivilstand { get; set; }
    public object[] adresseGradering { get; set; }
    public bool adresseSpesialtegn { get; set; }
    public bool ukjentBosted { get; set; }
    public bool harPostboks { get; set; }
    public bool borMedMor { get; set; }
    public bool borMedMedmor { get; set; }
    public bool borMedFar { get; set; }
    public bool foreldreHarSammeAdresse { get; set; }
    public bool harInnflytting { get; set; }
    public bool harUtflytting { get; set; }
    public bool harUtenlandskAdresseIFrittFormat { get; set; }
    public bool harPostadresseIFrittFormat { get; set; }
    public string sisteBoKommune { get; set; }
    public bool norskStatsborgerskap { get; set; }
    public bool flereStatsborgerskap { get; set; }
    public string[] statsborgerskap { get; set; }
    public bool harBostedsadresseHistorikk { get; set; }
    public bool harSivilstandHistorikk { get; set; }
    public bool harNavnHistorikk { get; set; }
    public bool harOpphold { get; set; }
}

public class Tenorrelasjoner
{
    public Freg[] freg { get; set; }
}

public class Freg
{
    public Tenormetadata tenorMetadata { get; set; }
    public string tenorRelasjonsnavn { get; set; }
    public string id { get; set; }
    public string kjoenn { get; set; }
    public string personstatus { get; set; }
    public string foedeland { get; set; }
    public string foedselsdato { get; set; }
    public string[] identifikator { get; set; }
    public string[] identifikatorType { get; set; }
    public object[] legitimasjonsdokument { get; set; }
    public bool falskIdentitet { get; set; }
    public object[] utenlandskPersonidentifikasjon { get; set; }
    public Hendelsermedsekven[] hendelserMedSekvens { get; set; }
    public string[] hendelser { get; set; }
    public string sisteHendelse { get; set; }
    public string visningnavn { get; set; }
    public string fornavn { get; set; }
    public string etternavn { get; set; }
    public int navnLengde { get; set; }
    public bool flereFornavn { get; set; }
    public string[] morFnr { get; set; }
    public string[] farFnr { get; set; }
    public string[] barnFnr { get; set; }
    public int antallBarn { get; set; }
    public bool harRelatertPersonUtenFolkeregisteridentifikator { get; set; }
    public bool harDoedfoedtBarn { get; set; }
    public string[] minRolleForPerson { get; set; }
    public string[] relatertPersonsRolle { get; set; }
    public bool harForeldreMedSammeKjoenn { get; set; }
    public bool harRettsligHandleevne { get; set; }
    public string sivilstand { get; set; }
    public string kommunenr { get; set; }
    public string bostedsadresse { get; set; }
    public string[] adresseGradering { get; set; }
    public bool adresseSpesialtegn { get; set; }
    public bool ukjentBosted { get; set; }
    public bool harPostboks { get; set; }
    public bool borMedMor { get; set; }
    public bool borMedMedmor { get; set; }
    public bool borMedFar { get; set; }
    public bool foreldreHarSammeAdresse { get; set; }
    public bool harInnflytting { get; set; }
    public bool harUtflytting { get; set; }
    public bool harUtenlandskAdresseIFrittFormat { get; set; }
    public bool harPostadresseIFrittFormat { get; set; }
    public string bostedsadresseType { get; set; }
    public bool norskStatsborgerskap { get; set; }
    public bool flereStatsborgerskap { get; set; }
    public string[] statsborgerskap { get; set; }
    public bool harBostedsadresseHistorikk { get; set; }
    public bool harSivilstandHistorikk { get; set; }
    public bool harNavnHistorikk { get; set; }
    public bool harOpphold { get; set; }
    public string partnerFnr { get; set; }
    public string doedsdato { get; set; }
    public string kontaktinfoDoedsbo { get; set; }
    public string sisteBoKommune { get; set; }
}

public class Tenormetadata
{
    public string id { get; set; }
    public object kilder { get; set; }
    public string kildedata { get; set; }
    public string kildeMime { get; set; }
    public int testdataVersjon { get; set; }
    public int datasettVersjon { get; set; }
    public string indeksert { get; set; }
    public DateTime oppdatert { get; set; }
    public DateTime opprettet { get; set; }
}

public class Hendelsermedsekven
{
    public string identifikator { get; set; }
    public string hendelse { get; set; }
    public int sekvens { get; set; }
}

public class Tenormetadata1
{
    public string id { get; set; }
    public string[] kilder { get; set; }
    public string kildedata { get; set; }
    public string kildeMime { get; set; }
    public int testdataVersjon { get; set; }
    public int datasettVersjon { get; set; }
    public string indeksert { get; set; }
    public DateTime oppdatert { get; set; }
    public DateTime opprettet { get; set; }
}

public class Hendelsermedsekven1
{
    public string identifikator { get; set; }
    public string hendelse { get; set; }
    public int sekvens { get; set; }
}
