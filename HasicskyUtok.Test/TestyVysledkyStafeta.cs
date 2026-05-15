namespace HasickyUtok.Test;

[TestClass]
public sealed class TestyVysledkyStafeta
{
    [TestMethod]
    public void TestMethodObaPlatneLepsiDruhy()
    {
        var vysledek = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 10),
            NeplatnyPokus1 = false,
            CasStafeta2 = new TimeOnly(0, 1, 20),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 1, Nazev = "Druzstvo 1", },
        };
        Assert.IsNotNull(vysledek);
        var vysledek2 = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 9),
            NeplatnyPokus1 = false,
            CasStafeta2 = new TimeOnly(0, 1, 8),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 2, Nazev = "Druzstvo 2", },
        };
        Assert.IsNotNull(vysledek2);

        var r = vysledek.CompareTo(vysledek2);
        Assert.IsGreaterThan(0, r);
    }
    [TestMethod]
    public void TestMethodObaPlatneLepsiPrvni()
    {
        var vysledek = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 7),
            NeplatnyPokus1 = false,
            CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 1, Nazev = "Druzstvo 1", },
        };
        Assert.IsNotNull(vysledek);
        var vysledek2 = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 9),
            NeplatnyPokus1 = false,
            CasStafeta2 = new TimeOnly(0, 1, 8),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 2, Nazev = "Druzstvo 2", },
        };
        Assert.IsNotNull(vysledek2);

        var r = vysledek.CompareTo(vysledek2);
        Assert.IsLessThan(0, r);
    }
    [TestMethod]
    public void TestMethodObaPlatneStejne()
    {
        var vysledek = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 7),
            NeplatnyPokus1 = false,
            CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 1, Nazev = "Druzstvo 1", },
        };
        Assert.IsNotNull(vysledek);
        var vysledek2 = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 7),
            NeplatnyPokus1 = false,
            CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 2, Nazev = "Druzstvo 2", },
        };
        Assert.IsNotNull(vysledek2);

        var r = vysledek.CompareTo(vysledek2);
        Assert.AreEqual(0, r);
    }
    [TestMethod]
    public void TestMethodPlatnyJeden()
    {
        var vysledek = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 7),
            NeplatnyPokus1 = true,
            CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 1, Nazev = "Druzstvo 1", },
        };
        Assert.IsNotNull(vysledek);
        var vysledek2 = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 7),
            NeplatnyPokus1 = false,
            CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = true,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 2, Nazev = "Druzstvo 2", },
        };
        Assert.IsNotNull(vysledek2);

        var r = vysledek.CompareTo(vysledek2);
        Assert.IsLessThan(0, r);
    }
    [TestMethod]
    public void TestMethodPlatnyJedenNezadaneCasy()
    {
        var vysledek = new HasicskyUtok.Models.VysledekStafeta
        {
            //CasStafeta1 = new TimeOnly(0, 1, 7),
            NeplatnyPokus1 = true,
            CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 1, Nazev = "Druzstvo 1", },
        };
        Assert.IsNotNull(vysledek);
        var vysledek2 = new HasicskyUtok.Models.VysledekStafeta
        {
            CasStafeta1 = new TimeOnly(0, 1, 7),
            NeplatnyPokus1 = false,
            //CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = true,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 2, Nazev = "Druzstvo 2", },
        };
        Assert.IsNotNull(vysledek2);

        var r = vysledek.CompareTo(vysledek2);
        Assert.IsLessThan(0, r);
    }
    [TestMethod]
    public void TestMethodPlatnyJedenDruhyNapletnePokusy()
    {
        var vysledek = new HasicskyUtok.Models.VysledekStafeta
        {
            NeplatnyPokus1 = true,
            CasStafeta2 = new TimeOnly(0, 1, 6),
            NeplatnyPokus2 = false,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 1, Nazev = "Druzstvo 1", },
        };
        Assert.IsNotNull(vysledek);
        var vysledek2 = new HasicskyUtok.Models.VysledekStafeta
        {
            NeplatnyPokus1 = true,
            NeplatnyPokus2 = true,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 2, Nazev = "Druzstvo 2", },
        };
        Assert.IsNotNull(vysledek2);

        var r = vysledek.CompareTo(vysledek2);

        Assert.IsLessThan(0, r);
    }
    [TestMethod]
    public void TestMethodNeplatneVsechny()
    {
        var vysledek = new HasicskyUtok.Models.VysledekStafeta
        {
            NeplatnyPokus1 = true,
            NeplatnyPokus2 = true,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 1, Nazev = "Druzstvo 1", },
        };
        Assert.IsNotNull(vysledek);
        var vysledek2 = new HasicskyUtok.Models.VysledekStafeta
        {
            NeplatnyPokus1 = true,
            NeplatnyPokus2 = true,
            Druzstvo = new HasicskyUtok.Models.Druzstvo { ID = 2, Nazev = "Druzstvo 2", },
        };
        Assert.IsNotNull(vysledek2);

        var r = vysledek.CompareTo(vysledek2);

        Assert.AreEqual(0, r);
    }
}
