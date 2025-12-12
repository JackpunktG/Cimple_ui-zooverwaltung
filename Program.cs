using System.Globalization;
using CimpleUI;

namespace Zooverwaltung;
public class Program
{
    public static void futter_ddm_populate(UIController uiC, DropdownMenu ddm, List<Futter> futterList)
    {
        string s = "";
        for (int i = 0; i < futterList.Count; ++i)
        {
            if (i == futterList.Count - 1)
            {
                Futter f = futterList[i];
                s += $"{f.Futtername}";
            }
            else
            {
                Futter f = futterList[i];
                s += $"{f.Futtername}\n";
            }
        }
        ddm.Populate(uiC, s);
    }

    public static void tierart_ddm_populate(UIController uiC, DropdownMenu ddm, List<Tierart> tierartList)
    {
        string s = "";
        for (int i = 0; i < tierartList.Count; ++i)
        {
            if (i == tierartList.Count - 1)
            {
                Tierart g = tierartList[i];
                s += $"{g.Bezeichnung}";
            }
            else
            {
                Tierart g = tierartList[i];
                s += $"{g.Bezeichnung}\n";
            }
        }
        ddm.Populate(uiC, s);
    }

    public static void tiere_ddm_populate(UIController uiC, DropdownMenu ddm, List<Tiere> tiereList)
    {
        string s = "";
        for (int i = 0; i < tiereList.Count; ++i)
        {
            if (i == tiereList.Count - 1)
            {
                Tiere g = tiereList[i];
                s += $"{g.Name} - id: {g.Id}";
            }
            else
            {
                Tiere g = tiereList[i];
                s += $"{g.Name} - id: {g.Id} \n";
            }
        }
        ddm.Populate(uiC, s);
    }
    public static void gehege_ddm_populate(UIController uiC, DropdownMenu ddm, List<Gehege> gehegeList, List<Kontinent> kontinentList)
    {
        string s = "";
        for (int i = 0; i < gehegeList.Count; ++i)
        {
            if (i == gehegeList.Count - 1)
            {
                Gehege g = gehegeList[i];
                s += g.Tostring_noID(kontinentList);
            }
            else
            {
                Gehege g = gehegeList[i];
                s += g.Tostring_noID(kontinentList) + "\n";
            }
        }
        ddm.Populate(uiC, s);
    }
    public static void kontinentDdm_populate(UIController uiC, DropdownMenu ddm, List<Kontinent> kontinentList)
    {
        string s = "";
        for (int i = 0; i < kontinentList.Count; ++i)
        {
            if (i == kontinentList.Count - 1)
            {
                Kontinent k = kontinentList[i];
                s += k.Tostring();
            }
            else
            {
                Kontinent k = kontinentList[i];
                s += k.Tostring() + "\n";
            }
        }
        ddm.Populate(uiC, s);
    }

    public static void Main(string[] args)
    {
        Database db = new("appuser", args[0]);
        List<Kontinent> kontinentList = new();
        db.Populate_kontinent(kontinentList);
        List<Tierart> tierartList = new();
        db.Populate_Tierart(tierartList);
        List<Gehege> gehegeList = new();
        db.Populate_gehege(gehegeList);
        List<Tiere> tierList = new();
        db.Populate_Tiere(tierList);
        List<Futter> futterList = new();
        db.Populate_futter(futterList);



        uint screenWidth = 1000;
        uint screenHeight = 800;
        bool running = true;
        using Arena mainArena = new();
        using StringMemory sm = new(mainArena, 1024);
        using FontHolder fh = new(mainArena, 3);
        fh.LoadFont("assets/fonts/ARIALMT.ttf", 16);
        fh.LoadFont("assets/fonts/ACADEROM.ttf", 16);
        fh.LoadFont("assets/fonts/AXCART.ttf", 16);

        using Arena arena = new();
        using Window window = new(arena, "Online Shop", screenWidth, screenHeight);
        using UIController uiC = new(arena, window, sm, fh, 1024);

        TabPannel tp = new(uiC, "Kontinent|Gehege|Tierart|Tiere|Futter|Uebersicht", TabPannelPossition.TABPANNEL_BUTTOM);

        //Constant Elements
        Button exit = new(uiC, (int)screenWidth - 100, 30, 80, 40, "Beenden", color: ColorRGBA.Red);
        exit.Clicked += () =>
        {
            //save logic
            running = false;
        };

        // Tab 1 - Kontinent
        bool kAktualisieren = false;
        TextField kAusgabenTb = new(uiC, screenWidth / 2, 190, 400, 50, tp, 1);

        Label kBezeichnungL = new(uiC, 20, 80, 120, 50, "Bezeichnung: ", tp, 1, 1, 22, ColorRGBA.White);
        TextBox kBezeichnungTb = new(uiC, 145, 90, 300, 50, tp, 1);
        Label kAusgabenL = new(uiC, 500, 140, 400, 50, "Kontinent Ausgaben:", tp, 1, color: ColorRGBA.White);

        Button kAddB = new(uiC, 40, 140, 300, 30, "Hinzufuegen / Aktualisieren", tp, 1, color: ColorRGBA.Green);

        Label kWahlL = new(uiC, 40, 280, 200, 50, "Waehl eine Kontinent ID aus: ", tp, 1, color: ColorRGBA.White);
        TextBox kWahlTb = new(uiC, 230, 300, 50, 50, tp, 1);

        Button kWahlB = new(uiC, 140, 350, 180, 30, "Bestaetigung", tp, 1, color: ColorRGBA.Blue);
        Button kLoschenB = new(uiC, 40, 190, 200, 30, "Loeschen", tp, 1, color: ColorRGBA.Red);
        Button kAllB = new(uiC, 40, 450, 200, 30, "Zeig alle Kontinenten", tp, 1, color: ColorRGBA.Blue);


        // Tab 2 - Gehege
        bool gAktualisieren = false;
        TextField gAusgabenTb = new(uiC, screenWidth / 2, 190, 400, 50, tp, 2);
        Label gAusgabenL = new(uiC, 500, 140, 400, 50, "Gehege Ausgaben:", tp, 2, color: ColorRGBA.White);

        Label gBezeichnungL = new(uiC, 20, 120, 120, 50, "Bezeichnung: ", tp, 2, 1, 22, ColorRGBA.White);
        TextBox gBezeichnungTb = new(uiC, 145, 130, 300, 50, tp, 2);

        Button gAddB = new(uiC, 40, 180, 300, 30, "Hinzufuegen / Aktualisieren", tp, 2, color: ColorRGBA.Green);

        //Label gWahlL = new(uiC, 40, 320, 200, 50, "Waehl eine Kontinent ID aus: ", tp, 2, color: ColorRGBA.White);
        //TextBox gWahlTb = new(uiC, 230, 340, 50, 50, tp, 2);

        // Button gWahlB = new(uiC, 140, 390, 180, 30, "Bestaetigung", tp, 2, color: ColorRGBA.Blue);
        Button gLoschenB = new(uiC, 40, 230, 200, 30, "Loeschen", tp, 2, color: ColorRGBA.Red);
        Button gAllB = new(uiC, 40, 490, 200, 30, "Zeig alle Gehege", tp, 2, color: ColorRGBA.Blue);
        DropdownMenu gGehegeDdm = new(uiC, "Gehege", 40, 380, 250, 30, tp, 2);
        gehege_ddm_populate(uiC, gGehegeDdm, gehegeList, kontinentList);
        DropdownMenu gKontinentDdm = new(uiC, "Waehl einen Kontinent", 145, 80, 300, 30, tp, 2);
        kontinentDdm_populate(uiC, gKontinentDdm, kontinentList);


        // Tabe 3 - Tierart
        bool tAktualisieren = false;
        TextField tAusgabenTb = new(uiC, screenWidth / 2, 190, 400, 50, tp, 3);

        Label tBezeichnungL = new(uiC, 20, 80, 120, 50, "Bezeichnung: ", tp, 3, 1, 22, ColorRGBA.White);
        TextBox tBezeichnungTb = new(uiC, 145, 90, 300, 50, tp, 3);
        Label tAusgabenL = new(uiC, 500, 140, 400, 50, "Tierart Ausgaben:", tp, 3, color: ColorRGBA.White);

        Button tAddB = new(uiC, 40, 140, 300, 30, "Hinzufuegen / Aktualisieren", tp, 3, color: ColorRGBA.Green);

        Label tWahlL = new(uiC, 40, 280, 200, 50, "Waehl eine Tierart ID aus: ", tp, 3, color: ColorRGBA.White);
        TextBox tWahlTb = new(uiC, 230, 300, 50, 50, tp, 3);

        Button tWahlB = new(uiC, 140, 350, 180, 30, "Bestaetigung", tp, 3, color: ColorRGBA.Blue);
        Button tLoschenB = new(uiC, 40, 190, 200, 30, "Loeschen", tp, 3, color: ColorRGBA.Red);
        Button tAllB = new(uiC, 40, 450, 200, 30, "Zeig alle Tierart", tp, 3, color: ColorRGBA.Blue);

        // Tab 4 - Tiere
        bool tierAktualisieren = false;
        TextField tierAusgabenTb = new(uiC, screenWidth / 2, 190, 400, 50, tp, 4);
        Label tierAusgabenL = new(uiC, 500, 140, 400, 50, "Tierart Ausgaben:", tp, 4, color: ColorRGBA.White);

        Label tierNameL = new(uiC, 20, 80, 120, 50, "Name: ", tp, 4, 1, 22, ColorRGBA.White);
        TextBox tierNameTb = new(uiC, 145, 90, 300, 50, tp, 4);
        Label tierGewichtL = new(uiC, 25, 140, 120, 50, "Gewicht: ", tp, 4, 1, 22, ColorRGBA.White);
        TextBox tierGewichtTb = new(uiC, 145, 150, 300, 50, tp, 4);
        Label tierGeburtL = new(uiC, 18, 190, 180, 50, "Geburtsdatum: ", tp, 4, 1, 22, ColorRGBA.White);
        TextBox tierGeburtTb = new(uiC, 205, 200, 240, 50, tp, 4);

        Button tierAddB = new(uiC, 40, 360, 300, 30, "Hinzufuegen / Aktualisieren", tp, 4, color: ColorRGBA.Green);
        Button tierLoschenB = new(uiC, 40, 400, 200, 30, "Loeschen", tp, 4, color: ColorRGBA.Red);

        Button tierAllB = new(uiC, 40, 680, 200, 30, "Zeig alle Tiere", tp, 4, color: ColorRGBA.Blue);
        Label tierSucheL = new(uiC, 40, 515, 400, 30, "Suche tiere, waehlen eine suche feld und geben Sie ihre option", tp, 4, color: ColorRGBA.Cyan);
        TextBox tierSucheTb = new(uiC, 200, 550, 250, 30, tp, 4);
        Button tierSucheB = new(uiC, 400, 590, 50, 30, "Suche", tp, 4, color: ColorRGBA.Purple);
        DropdownMenu tierSucheDdm = new(uiC, "Suche nach", 40, 550, 150, 30, tp, 4);


        DropdownMenu tierGehegeDdm = new(uiC, "Gehege", 145, 300, 300, 30, tp, 4);
        DropdownMenu tierArtDdm = new(uiC, "Tierart", 145, 250, 300, 30, tp, 4);

        DropdownMenu tierTierDdm = new(uiC, "Tier auswahl", 50, 20, 400, 20, tp, 4);


        // Tab 5 - Futter
        //bool fAktualisieren = false;
        Label fLabelL = new(uiC, 20, 60, 150, 50, "Neu futter: ", tp, 5, color: ColorRGBA.White);
        TextBox fNameTb = new(uiC, 160, 70, 300, 30, tp, 5);
        Button fAddB = new(uiC, 200, 130, 200, 30, "Hinzufuegen", tp, 5, color: ColorRGBA.Green);
        Label fAusgabenL = new(uiC, 50, 250, 400, 50, "Futter sorten:", tp, 5, color: ColorRGBA.White);
        TextField fAusgabenTb = new(uiC, 50, 300, 400, 50, tp, 5);
        Button fLoschenB = new(uiC, 20, 130, 150, 30, "Loeschen", tp, 5, color: ColorRGBA.Red);
        Button fAllB = new(uiC, 20, 180, 100, 50, "Zeig alle Futter", tp, 5, color: ColorRGBA.Blue);

        TextField fVerbindAusgabenTb = new(uiC, screenWidth / 2, 370, 400, 50, tp, 5);
        DropdownMenu fTierfutterDdm = new(uiC, "Tier Futter suchen", 500, 330, 250, 30, tp, 5);

        Label fVerbindL = new(uiC, 500, 110, 400, 50, "Waehl eine Teir und eine Futter", tp, 5, color: ColorRGBA.White);
        Button fVerbindB = new(uiC, 720, 240, 180, 20, "Futter zuweisen", tp, 5, color: ColorRGBA.Green);
        Button fEntfernenB = new(uiC, 720, 280, 180, 20, "Futter entfernen", tp, 5, color: ColorRGBA.Red);
        DropdownMenu fFutterDdm = new(uiC, "Futter auswahl", 500, 200, 400, 20, tp, 5);
        DropdownMenu fTierDdm = new(uiC, "Tier auswahl", 500, 160, 400, 20, tp, 5);



        //Tab 6 - Uebersicht
        Label uLabelL = new(uiC, 20, 50, 450, 30, "Geben Sie eine SELECT Anfrage dirket zum Datenbank", tp, 6, color: ColorRGBA.White);
        TextBox uAnfrageTB = new(uiC, 30, 80, 600, 50, tp, 6);
        Button uAnfragB = new(uiC, 650, 80, 60, 30, "Senden", tp, 6, color: ColorRGBA.Purple);
        Button uExportB = new(uiC, 900, 150, 80, 30, "Export CSV", tp, 6, color: ColorRGBA.Olive);

        TextField uAusgabenTb = new(uiC, 30, 200, 930, 100, tp, 6);



        //Tab 1 Logic
        kAddB.Clicked += () =>
        {
            string bezeichnung = kBezeichnungTb.ToText();
            if (string.IsNullOrWhiteSpace(bezeichnung))
                PopupNotice.Create(uiC, "Bezeichnung ist Leer!!", "x");
            else
            {
                Kontinent k = new(bezeichnung);
                if (kAktualisieren)
                {
                    if (int.TryParse(kWahlTb.ToText(), out int id))
                        db.Kontinent_update(k, id);
                    else
                        PopupNotice.Create(uiC, "Fehler beim kAktualisieren", "x");
                }
                else
                    db.Kontinent_add(k);

                db.Populate_kontinent(kontinentList);
                kAusgabenTb.Clear();
                kAusgabenTb.AppendText(Kontinent.List_to_string(kontinentList));
            }
            kontinentDdm_populate(uiC, gKontinentDdm, kontinentList);
            kBezeichnungTb.Clear();
            kAktualisieren = false;
            kWahlTb.Clear();
        };

        kLoschenB.Clicked += () =>
        {
            if (kAktualisieren)
            {
                if (int.TryParse(kWahlTb.ToText(), out int id))
                {
                    string bezeichnung = kBezeichnungTb.ToText();
                    Kontinent? k = kontinentList.FirstOrDefault
                        (w => w.Bezeichnung == bezeichnung && w.Id == id);
                    if (k == null)
                        PopupNotice.Create(uiC, "Fehler beim loeschen", "x");
                    else
                    {
                        db.Kontinent_delete(id);
                        db.Populate_kontinent(kontinentList);
                        kAusgabenTb.Clear();
                        kAusgabenTb.AppendText(Kontinent.List_to_string(kontinentList));
                    }
                }
            }
            else
                PopupNotice.Create(uiC, "Fehler beim loeschen", "x");

            kBezeichnungTb.Clear();
            kAktualisieren = false;
            kWahlTb.Clear();
            kontinentDdm_populate(uiC, gKontinentDdm, kontinentList);

        };


        kAllB.Clicked += () =>
        {
            kAktualisieren = false;
            kBezeichnungTb.Clear();
            kWahlTb.Clear();
            db.Populate_kontinent(kontinentList);
            kAusgabenTb.Clear();
            kAusgabenTb.AppendText(Kontinent.List_to_string(kontinentList));
        };


        kWahlB.Clicked += () =>
        {
            kAktualisieren = false;
            if (int.TryParse(kWahlTb.ToText(), out int id))
            {
                db.Populate_kontinent(kontinentList);
                Kontinent? k = kontinentList.FirstOrDefault(w => w.Id == id);
                if (k == null)
                {
                    PopupNotice.Create(uiC, $"Kein Kontinent gefunden mit ID '{id}'", "x");
                    kWahlTb.Clear();
                }
                else
                {
                    kBezeichnungTb.Clear();
                    kBezeichnungTb.AppendText(k.Bezeichnung);
                    kAktualisieren = true;
                    PopupNotice.Create(uiC, "Sie koennen jetzt die Bezeichnung enderen dann kAktualisieren, oder loeschen", "x");
                }

            }
            else
            {
                PopupNotice.Create(uiC, "Fehler Beim ID parsen", "x");
                kWahlTb.Clear();
            }
        };


        //tab 2 - logic
        int gKontIndex = -1;

        gKontinentDdm.Selected += () =>
        {
            gKontIndex = gKontinentDdm.SelectedIndex;
        };

        gAddB.Clicked += () =>
        {
            string bezeichnung = gBezeichnungTb.ToText();
            if (string.IsNullOrWhiteSpace(bezeichnung))
                PopupNotice.Create(uiC, "Bezeichnung ist Leer!!", "x");
            else
            {
                int? k_id = gKontIndex == -1 ? null : kontinentList[gKontIndex].Id;
                Gehege g = new(bezeichnung, k_id);
                if (gAktualisieren)
                    db.Gehege_update(g, gehegeList[gGehegeDdm.SelectedIndex].Id);
                else
                    db.Gehege_add(g);

                db.Populate_gehege(gehegeList);
                gehege_ddm_populate(uiC, gGehegeDdm, gehegeList, kontinentList);
                gAusgabenTb.Clear();
                gAusgabenTb.AppendText(Gehege.List_to_string(gehegeList, kontinentList));
            }
            gGehegeDdm.Reset();
            gKontinentDdm.Reset();
            gKontIndex = -1;
            gBezeichnungTb.Clear();
            gAktualisieren = false;
        };

        gGehegeDdm.Selected += () =>
        {
            gBezeichnungTb.Clear();
            byte? k_id = (byte?)gehegeList[gGehegeDdm.SelectedIndex].K_id;
            if (k_id != null)
            {
                for (int i = 0; i < 256; ++i)
                {
                    if (kontinentList[i].Id == k_id)
                    {
                        gKontinentDdm.Select((byte)i);
                        break;
                    }
                }
            }
            else
                gKontinentDdm.Reset();

            gBezeichnungTb.AppendText(gehegeList[gGehegeDdm.SelectedIndex].Bezeichnung);

            PopupNotice.Create(uiC, "Sie koennen jetzt die Gehege aendern oder loeschen", "x");
            gAktualisieren = true;
        };

        gLoschenB.Clicked += () =>
        {
            if (gAktualisieren)
            {
                bool correct = true;
                int index = gGehegeDdm.SelectedIndex;
                if (index == -1)
                    correct = false;
                else
                {
                    Gehege g = gehegeList[gGehegeDdm.SelectedIndex];
                    if (g.Bezeichnung == gBezeichnungTb.ToText())
                        db.Gehege_delete(g.Id);
                    else
                        correct = false;
                }

                if (!correct)
                    PopupNotice.Create(uiC, "Fehler biem loeschen", "x");
                else
                {
                    db.Populate_gehege(gehegeList);
                    gehege_ddm_populate(uiC, gGehegeDdm, gehegeList, kontinentList);
                    gAusgabenTb.Clear();
                    gAusgabenTb.AppendText(Gehege.List_to_string(gehegeList, kontinentList));
                }
            }
            gAktualisieren = false;
            gGehegeDdm.Reset();
            gKontinentDdm.Reset();
            gBezeichnungTb.Clear();
        };

        gAllB.Clicked += () =>
        {
            db.Populate_gehege(gehegeList);
            gehege_ddm_populate(uiC, gGehegeDdm, gehegeList, kontinentList);
            gAusgabenTb.Clear();
            gAusgabenTb.AppendText(Gehege.List_to_string(gehegeList, kontinentList));
        };


        //tab 3 - logic
        tAddB.Clicked += () =>
            {
                string bezeichnung = tBezeichnungTb.ToText();
                if (string.IsNullOrWhiteSpace(bezeichnung))
                    PopupNotice.Create(uiC, "Bezeichnung ist Leer!!", "x");
                else
                {
                    Tierart t = new(bezeichnung);
                    if (tAktualisieren)
                    {
                        if (int.TryParse(tWahlTb.ToText(), out int id))
                            db.Tierart_update(t, id);
                        else
                            PopupNotice.Create(uiC, "Fehler beim Aktualisieren", "x");
                    }
                    else
                        db.Tierart_add(t);

                    db.Populate_Tierart(tierartList);
                    tAusgabenTb.Clear();
                    tAusgabenTb.AppendText(Tierart.List_to_string(tierartList));
                }
                tBezeichnungTb.Clear();
                tAktualisieren = false;
                tWahlTb.Clear();
            };

        tLoschenB.Clicked += () =>
        {
            if (tAktualisieren)
            {
                if (int.TryParse(tWahlTb.ToText(), out int id))
                {
                    string bezeichnung = tBezeichnungTb.ToText();
                    Tierart? t = tierartList.FirstOrDefault
                        (w => w.Bezeichnung == bezeichnung && w.Id == id);
                    if (t == null)
                        PopupNotice.Create(uiC, "Fehler beim loeschen", "x");
                    else
                    {
                        db.Tierart_delete(id);
                        db.Populate_Tierart(tierartList);
                        tAusgabenTb.Clear();
                        tAusgabenTb.AppendText(Tierart.List_to_string(tierartList));
                    }
                }
            }
            else
                PopupNotice.Create(uiC, "Fehler beim loeschen", "x");

            tBezeichnungTb.Clear();
            tAktualisieren = false;
            tWahlTb.Clear();

        };


        tAllB.Clicked += () =>
        {
            tAktualisieren = false;
            tBezeichnungTb.Clear();
            tWahlTb.Clear();
            db.Populate_Tierart(tierartList);
            tAusgabenTb.Clear();
            tAusgabenTb.AppendText(Tierart.List_to_string(tierartList));
        };


        tWahlB.Clicked += () =>
        {
            tAktualisieren = false;
            if (int.TryParse(tWahlTb.ToText(), out int id))
            {
                db.Populate_Tierart(tierartList);
                Tierart? t = tierartList.FirstOrDefault(w => w.Id == id);
                if (t == null)
                {
                    PopupNotice.Create(uiC, $"Kein Kontinent gefunden mit ID '{id}'", "x");
                    tWahlTb.Clear();
                }
                else
                {
                    tBezeichnungTb.Clear();
                    tBezeichnungTb.AppendText(t.Bezeichnung);
                    tAktualisieren = true;
                    PopupNotice.Create(uiC, "Sie koennen jetzt die Bezeichnung enderen dann kAktualisieren, oder loeschen", "x");
                }

            }
            else
            {
                PopupNotice.Create(uiC, "Fehler Beim ID parsen", "x");
                tWahlTb.Clear();
            }
        };

        // Tiere logic -  Tab 4
        string[] tierSucheFelde = { "Tierart", "Gehege", "Name", "gewicht ueber", "gewicht unter" };
        tierSucheDdm.Populate(uiC, $"{tierSucheFelde[0]}\n{tierSucheFelde[1]}\n{tierSucheFelde[2]}\n{tierSucheFelde[3]}\n{tierSucheFelde[4]}");
        string[] dateFormat = { "dd.MM.yyyy", "dd.M.yyyy", "d.M.yyyy", "d.MM.yyyy" };
        tiere_ddm_populate(uiC, tierTierDdm, tierList);
        gehege_ddm_populate(uiC, tierGehegeDdm, gehegeList, kontinentList);
        tierart_ddm_populate(uiC, tierArtDdm, tierartList);

        tierSucheB.Clicked += () =>
        {
            string suche = tierSucheTb.ToText();
            if (string.IsNullOrWhiteSpace(suche))
            {
                PopupNotice.Create(uiC, "Suche option ist leer", "x");
                tierSucheTb.Clear();
                return;
            }

            db.Populate_Tiere(tierList);
            string ausgabe = "";
            switch (tierSucheDdm.SelectedIndex)
            {
                case 0:
                    {
                        int id = -1;
                        for (int i = 0; i < tierartList.Count; ++i)
                        {
                            if (string.Equals(tierartList[i].Bezeichnung, suche, StringComparison.OrdinalIgnoreCase))
                                id = tierartList[i].Id;
                        }
                        var tier = tierList.Where(w => w.Tierart_id == (id == -1 ? (int?)null : id)).ToList();
                        if (tier.Count > 0)
                            ausgabe = Tiere.To_list_string(tier, tierartList, gehegeList);
                        else
                            ausgabe = "Kein Tier gefunden";
                        break;
                    }
                case 1:
                    {
                        int id = -1;
                        for (int i = 0; i < gehegeList.Count; ++i)
                        {
                            if (string.Equals(gehegeList[i].Bezeichnung, suche, StringComparison.OrdinalIgnoreCase))
                                id = tierartList[i].Id;
                        }
                        var tier = tierList.Where(w => w.Gehege_id == (id == -1 ? (int?)null : id)).ToList();
                        if (tier.Count > 0)
                            ausgabe = Tiere.To_list_string(tier, tierartList, gehegeList);
                        else
                            ausgabe = "Kein Tier gefunden";
                        break;
                    }
                case 2:
                    {
                        var tier = tierList.Where(w => string.Equals(w.Name, suche, StringComparison.OrdinalIgnoreCase)).ToList();
                        if (tier.Count > 0)
                            ausgabe = Tiere.To_list_string(tier, tierartList, gehegeList);
                        else
                            ausgabe = "Kein Tier gefunden";
                        break;
                    }
                case 3:
                case 4:
                    {
                        bool unter = tierSucheDdm.SelectedIndex == 4;
                        if (float.TryParse(suche, out float gewicht))
                        {
                            if (unter)
                            {
                                var tier = tierList.Where(w => w.Gewicht < gewicht).ToList(); if (tier.Count > 0)
                                    ausgabe = Tiere.To_list_string(tier, tierartList, gehegeList);
                                else
                                    ausgabe = "Kein Tier gefunden";
                            }
                            else
                            {
                                var tier = tierList.Where(w => w.Gewicht > gewicht).ToList(); if (tier.Count > 0)
                                    ausgabe = Tiere.To_list_string(tier, tierartList, gehegeList);
                                else
                                    ausgabe = "Kein Tier gefunden";
                            }
                        }
                        else
                            PopupNotice.Create(uiC, "Felher beim gewicht eingabe", "x");

                        break;
                    }
                default:
                    PopupNotice.Create(uiC, "Kein suche feld gewaehlt", "x");
                    return;

            }
            tierAusgabenTb.Clear();
            tierAusgabenTb.AppendText(ausgabe);
        };

        tierTierDdm.Selected += () =>
        {
            Tiere t = tierList[tierTierDdm.SelectedIndex];
            tierNameTb.Clear();
            tierNameTb.AppendText($"{t.Name}");
            tierGewichtTb.Clear();
            tierGewichtTb.AppendText($"{t.Gewicht}");
            tierGeburtTb.Clear();
            tierGeburtTb.AppendText($"{t.Geburtstag.Day}.{t.Geburtstag.Month}.{t.Geburtstag.Year}");

            for (int i = 0; i < tierartList.Count; ++i)
            {
                if (tierartList[i].Id == t.Tierart_id)
                {
                    tierArtDdm.Select((byte)i);
                    break;
                }

            }
            for (int i = 0; i < gehegeList.Count; ++i)
            {
                if (gehegeList[i].Id == t.Gehege_id)
                {
                    tierGehegeDdm.Select((byte)i);
                    break;
                }
            }
            tierAktualisieren = true;
        };

        tierAddB.Clicked += () =>
        {
            string name = tierNameTb.ToText();

            if (string.IsNullOrWhiteSpace(name))
            {
                PopupNotice.Create(uiC, "Name ist leer!", "x");
                tierNameTb.Clear();
                return;
            }

            float gewicht;
            if (!float.TryParse(tierGewichtTb.ToText(), out gewicht))
            {
                PopupNotice.Create(uiC, "Fehler beim gewicht. Achtung erwartet: 'xx.xx'", "x");
                return;
            }

            DateTime geburtstag;
            if (!DateTime.TryParseExact(tierGeburtTb.ToText(), dateFormat, CultureInfo.InvariantCulture,
                             DateTimeStyles.None, out geburtstag))
            {
                PopupNotice.Create(uiC, "Fehler beim geburtsdatum. Achtung erwartet: 'DD.MM.YYYY'", "x");
                return;
            }



            Tiere t = new(name, gewicht, geburtstag, tierArtDdm.SelectedIndex == -1 ? null : tierartList[tierArtDdm.SelectedIndex].Id,
                            tierGehegeDdm.SelectedIndex == -1 ? null : gehegeList[tierGehegeDdm.SelectedIndex].Id);
            if (tierAktualisieren)
                db.Tier_update(t, tierList[tierTierDdm.SelectedIndex].Id);
            else
                db.Tier_add(t);

            tierNameTb.Clear();
            tierGewichtTb.Clear();
            tierGeburtTb.Clear();
            tierArtDdm.Reset();
            tierGehegeDdm.Reset();

            db.Populate_Tiere(tierList);
            tierAusgabenTb.Clear();
            tierAusgabenTb.AppendText(Tiere.To_list_string(tierList, tierartList, gehegeList));
            tiere_ddm_populate(uiC, tierTierDdm, tierList);
            tiere_ddm_populate(uiC, fTierDdm, tierList);
            tiere_ddm_populate(uiC, fTierfutterDdm, tierList);

            tierAktualisieren = false;

        };

        tierLoschenB.Clicked += () =>
        {
            if (tierAktualisieren)
            {
                int id = tierList[tierTierDdm.SelectedIndex].Id;

                string name = tierNameTb.ToText();

                if (string.IsNullOrWhiteSpace(name))
                {
                    PopupNotice.Create(uiC, "Name ist leer!", "x");
                    tierNameTb.Clear();
                    return;
                }

                float gewicht;
                if (!float.TryParse(tierGewichtTb.ToText(), out gewicht))
                {
                    PopupNotice.Create(uiC, "Fehler beim gewicht. Achtung erwartet: 'xx.xx'", "x");
                    return;
                }

                DateTime geburtstag;
                if (!DateTime.TryParseExact(tierGeburtTb.ToText(), dateFormat, CultureInfo.InvariantCulture,
                                 DateTimeStyles.None, out geburtstag))
                {
                    PopupNotice.Create(uiC, "Fehler beim geburtsdatum. Achtung erwartet: 'DD.MM.YYYY'", "x");
                    return;
                }

                Tiere t = new(name, gewicht, geburtstag, tierArtDdm.SelectedIndex == -1 ? null : tierartList[tierArtDdm.SelectedIndex].Id,
                            tierGehegeDdm.SelectedIndex == -1 ? null : gehegeList[tierGehegeDdm.SelectedIndex].Id, id);
                if (t.Equals(tierList[tierTierDdm.SelectedIndex]))
                    db.Tier_delete(id);
                else
                    PopupNotice.Create(uiC, "Fehler beim loeschen!", "x");

                tierNameTb.Clear();
                tierGewichtTb.Clear();
                tierGeburtTb.Clear();
                tierArtDdm.Reset();
                tierGehegeDdm.Reset();

                db.Populate_Tiere(tierList);
                tierAusgabenTb.Clear();
                tierAusgabenTb.AppendText(Tiere.To_list_string(tierList, tierartList, gehegeList));
                tiere_ddm_populate(uiC, tierTierDdm, tierList);

                tierAktualisieren = false;
                tiere_ddm_populate(uiC, fTierDdm, tierList);
                tiere_ddm_populate(uiC, fTierfutterDdm, tierList);
            }

        };

        tierAllB.Clicked += () =>
        {
            tierAusgabenTb.Clear();
            db.Populate_Tiere(tierList);
            tierAusgabenTb.AppendText(Tiere.To_list_string(tierList, tierartList, gehegeList));
            tierAktualisieren = false;

        };

        // tab 5 - logic
        futter_ddm_populate(uiC, fFutterDdm, futterList);
        tiere_ddm_populate(uiC, fTierDdm, tierList);
        tiere_ddm_populate(uiC, fTierfutterDdm, tierList);

        fAddB.Clicked += () =>
        {
            string name = fNameTb.ToText();
            if (string.IsNullOrWhiteSpace(name))
            {
                PopupNotice.Create(uiC, "Futter name ist leer!", "x");
                fNameTb.Clear();
                return;
            }

            Futter f = new(name);
            db.Futter_add(f);
            fNameTb.Clear();
            db.Populate_futter(futterList);
            fAusgabenTb.Clear();
            fAusgabenTb.AppendText(Futter.List_to_string(futterList));
            futter_ddm_populate(uiC, fFutterDdm, futterList);
        };

        fLoschenB.Clicked += () =>
        {
            string name = fNameTb.ToText();
            Futter? f = futterList.FirstOrDefault(w => w.Futtername == name);
            if (f == null)
            {
                PopupNotice.Create(uiC, "Fehler beim loeschen!", "x");
                fNameTb.Clear();
                return;
            }
            db.Futter_delete(f.Id);
            db.Populate_futter(futterList);
            fAusgabenTb.Clear();
            fNameTb.Clear();
            fAusgabenTb.AppendText(Futter.List_to_string(futterList));
            futter_ddm_populate(uiC, fFutterDdm, futterList);
        };

        fFutterDdm.Selected += () =>
        {
        };

        fAllB.Clicked += () =>
        {
            db.Populate_futter(futterList);
            fAusgabenTb.Clear();
            fAusgabenTb.AppendText(Futter.List_to_string(futterList));
        };

        fVerbindB.Clicked += () =>
        {
            if (fFutterDdm.SelectedIndex == -1 || fTierDdm.SelectedIndex == -1)
            {
                PopupNotice.Create(uiC, "Fehler beim zuweisen. Waehl eine Tier und eine Futter aus!", "x");
                return;
            }

            int tier_id = tierList[fTierDdm.SelectedIndex].Id;
            int futter_id = futterList[fFutterDdm.SelectedIndex].Id;

            db.Tier_futter_add(tier_id, futter_id); fTierfutterDdm.Reset();
            fVerbindAusgabenTb.Clear();
        };

        fEntfernenB.Clicked += () =>
        {
            if (fFutterDdm.SelectedIndex == -1 || fTierDdm.SelectedIndex == -1)
            {
                PopupNotice.Create(uiC, "Fehler beim entfernen. Waehl eine Tier und eine Futter aus!", "x");
                return;
            }

            int tier_id = tierList[fTierDdm.SelectedIndex].Id;
            int futter_id = futterList[fFutterDdm.SelectedIndex].Id;

            db.Tier_futter_delete(tier_id, futter_id);
            fTierfutterDdm.Reset();
            fVerbindAusgabenTb.Clear();
        };

        fTierfutterDdm.Selected += () =>
        {
            if (fTierfutterDdm.SelectedIndex == -1)
                return;

            int tier_id = tierList[fTierfutterDdm.SelectedIndex].Id;
            fVerbindAusgabenTb.Clear();
            fVerbindAusgabenTb.AppendText(db.Tier_futter_list(tier_id));
        };



        // tab 6 - Logic
        uAnfragB.Clicked += () =>
        {

            string anfrage = uAnfrageTB.ToText();
            if (string.IsNullOrWhiteSpace(anfrage))
            {
                PopupNotice.Create(uiC, "Anfrage satz ist leer!", "x");
                uAnfrageTB.Clear();
                return;
            }

            string ausgabe = "";
            if (!db.Select_Anfrage(anfrage, out ausgabe))
                PopupNotice.Create(uiC, "Fehler beim Anfrage!!", "x");

            uAusgabenTb.Clear();
            uAusgabenTb.AppendText(ausgabe);
            uAnfrageTB.Clear();
        };

        uExportB.Clicked += () =>
        {
            string ausgabe = uAusgabenTb.ToText();
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            if (string.IsNullOrWhiteSpace(ausgabe))
                PopupNotice.Create(uiC, "Fehler beim Exportieren!!", "x");
            else
            {
                File.WriteAllText($"zoo_export_{timestamp}.csv", ausgabe);
                PopupNotice.Create(uiC, $"Erfolgreich exportiert in 'zoo_export_{timestamp}.csv'", "x");
            }
        };

        while (running)
        {
            uiC.EventCheck();
            uiC.Update(0.16f);
            if (fTierfutterDdm.State == DropdownMenu_State.DROPDOWN_NORMAL)
                fVerbindAusgabenTb.Clear();
            uiC.EasyRender();

        }
    }
}
