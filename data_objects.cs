using System.Text;
using Npgsql;

namespace Zooverwaltung
{
    public enum WITH_ID
    {
        NO_ID,
        WITH_ID
    };

    public class Kontinent
    {
        public int Id { get; set; }
        public string Bezeichnung { get; set; }

        public Kontinent(string bezeichnung, int id = -1)
        {
            Id = id;
            Bezeichnung = bezeichnung;
        }

        public override string ToString()
        {
            return $"id: {Id} {Bezeichnung}";
        }

        public string Tostring()
        {
            return $"{Bezeichnung}";
        }

        public static string List_to_string(List<Kontinent> kontinent)
        {
            string result = "";
            foreach (Kontinent k in kontinent)
            {
                result += k.ToString() + "\n";
            }
            return result;
        }
    }

    public class Gehege
    {
        public int Id { get; set; }
        public string Bezeichnung { get; set; }
        public int? K_id { get; set; }

        public Gehege(string bezeichnung, int? k_id = null, int id = -1)
        {
            Id = id;
            Bezeichnung = bezeichnung;
            K_id = k_id;
        }

        public string Tostring(List<Kontinent> kontinente)
        {
            Kontinent? k = kontinente.FirstOrDefault(w => w.Id == K_id);
            string kontinent = k == null ? "kein kontinent angegeben" : k.Bezeichnung;
            return $"id: {Id}, {Bezeichnung} - {kontinent}";
        }

        public static string List_to_string(List<Gehege> gehegeList, List<Kontinent> k)
        {
            string result = "";
            foreach (Gehege g in gehegeList)
            {
                result += g.Tostring(k) + "\n";
            }
            return result;
        }

        public string Tostring_noID(List<Kontinent> kontinente)
        {
            Kontinent? k = kontinente.FirstOrDefault(w => w.Id == K_id);
            string kontinent = k == null ? "kein kontinent angegeben" : k.Bezeichnung;
            return $"{Bezeichnung} - {kontinent}";
        }
    }


    public class Tierart
    {
        public int Id { get; set; }
        public string Bezeichnung { get; set; }

        public Tierart(string bezeichnung, int id = -1)
        {
            Id = id;
            Bezeichnung = bezeichnung;
        }

        public override string ToString()
        {
            return $"id: {Id}, {Bezeichnung}";
        }

        public static string List_to_string(List<Tierart> tierart)
        {
            string result = "";
            foreach (Tierart t in tierart)
            {
                result += t.ToString() + "\n";
            }
            return result;
        }
    }

    public class Tiere
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Gewicht { get; set; }
        public DateTime Geburtstag { get; set; }
        public int? Tierart_id { get; set; }
        public int? Gehege_id { get; set; }

        public Tiere(string name, float gewicht, DateTime geburtstag, int? t_id = null, int? g_id = null, int id = -1)
        {
            Id = id;
            Name = name;
            Gewicht = gewicht;
            Geburtstag = geburtstag;
            Tierart_id = t_id;
            Gehege_id = g_id;
        }

        public bool Equals(Tiere obj)
        {
            return Name == obj.Name &&
                   Gewicht == obj.Gewicht &&
                   Geburtstag == obj.Geburtstag &&
                   Tierart_id == obj.Tierart_id &&
                   Gehege_id == obj.Gehege_id;
        }

        public string To_string(List<Tierart> art, List<Gehege> gehege)
        {
            string s = "";
            Tierart? t = art.FirstOrDefault(w => w.Id == Tierart_id);
            if (t == null)
                s = "kein tierart eingetragen";
            else
                s = t.Bezeichnung;
            Gehege? g = gehege.FirstOrDefault(w => w.Id == Gehege_id);
            if (g == null)
                s += ", nicht gerade im gehege";
            else
                s += $", {g.Bezeichnung}";

            return $"id: {Id} {Name}, D.O.B: {Geburtstag.Day}.{Geburtstag.Month}.{Geburtstag.Year} Gewicht: {Gewicht} - {s}";
        }

        public static string To_list_string(List<Tiere> tier, List<Tierart> tierart, List<Gehege> gehege)
        {
            string result = "";
            foreach (Tiere t in tier)
            {
                result += t.To_string(tierart, gehege) + "\n";
            }
            return result;
        }
    }


    public class Database
    {
        private readonly string connString;

        public Database(string username, string password)
        {
            connString = $"Host=localhost;Username={username};Password={password};Database=zooverwaltung";
        }
        public bool TestConnection()
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                Console.WriteLine("Verbindung zur Datenbank hergestellt.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                return false;
            }
        }

        public void Populate_gehege(List<Gehege> gehege)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();


            gehege.Clear();
            using (var cmd = new NpgsqlCommand("SELECT * FROM gehege", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    gehege.Add(new Gehege(
                        reader.GetString(1),
                        reader.IsDBNull(2) ? null : reader.GetInt32(2),
                        reader.GetInt32(0)
                    ));
                }
            }
        }

        public void Populate_kontinent(List<Kontinent> kontinent)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            kontinent.Clear();
            using (var cmd = new NpgsqlCommand("SELECT * FROM kontinent", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    kontinent.Add(new Kontinent(
                        reader.GetString(1),
                        reader.GetInt32(0)
                    ));
                }
            }
        }

        public void Populate_Tierart(List<Tierart> tierart)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();


            tierart.Clear();
            using (var cmd = new NpgsqlCommand("SELECT * FROM tierart", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tierart.Add(new Tierart(
                        reader.GetString(1),
                        reader.GetInt32(0)
                    ));
                }
            }
        }

        public void Populate_Tiere(List<Tiere> tiere)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            tiere.Clear();
            using (var cmd = new NpgsqlCommand("SELECT * FROM tiere", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tiere.Add(new Tiere(
                        reader.GetString(1),
                        (float)reader.GetDecimal(2),
                        reader.GetDateTime(3),
                        reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        reader.GetInt32(0)
                    ));
                }
            }
        }

        public void Kontinent_add(Kontinent k)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO kontinent(bezeichnung) VALUES (@bezeichnung)", conn);
            cmd.Parameters.AddWithValue("bezeichnung", k.Bezeichnung);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Kontinent_update(Kontinent k, int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE kontinent SET bezeichnung = @bezeichnung WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("bezeichnung", k.Bezeichnung);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Kontinent_delete(int id)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM kontinent WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Gehege_add(Gehege g)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO gehege(bezeichnung, k_id) VALUES (@bezeichnung, @k_id)", conn);
            cmd.Parameters.AddWithValue("bezeichnung", g.Bezeichnung);
            if (g.K_id.HasValue)
                cmd.Parameters.AddWithValue("k_id", g.K_id.Value);
            else
                cmd.Parameters.AddWithValue("k_id", DBNull.Value);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }

        }

        public void Gehege_update(Gehege g, int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE gehege SET bezeichnung = @bezeichnung, k_id = @k_id WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("bezeichnung", g.Bezeichnung);
            if (g.K_id.HasValue)
                cmd.Parameters.AddWithValue("k_id", g.K_id.Value);
            else
                cmd.Parameters.AddWithValue("k_id", DBNull.Value);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Gehege_delete(int id)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM gehege WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Tierart_add(Tierart t)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO tierart(bezeichnung) VALUES (@bezeichnung)", conn);
            cmd.Parameters.AddWithValue("bezeichnung", t.Bezeichnung);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Tierart_update(Tierart t, int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE tierart SET bezeichnung = @bezeichnung WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("bezeichnung", t.Bezeichnung);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Tierart_delete(int id)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM tierart WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Tier_add(Tiere t)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO tiere(name, gewicht, geburtsdatum, tier_art_id, gehege_id) VALUES(@name, @gewicht, @geburtsdatum, @tier_art_id, @gehege_id)", conn);
            cmd.Parameters.AddWithValue("name", t.Name);
            cmd.Parameters.AddWithValue("gewicht", t.Gewicht);
            cmd.Parameters.AddWithValue("geburtsdatum", t.Geburtstag);
            if (t.Tierart_id.HasValue)
                cmd.Parameters.AddWithValue("tier_art_id", t.Tierart_id);
            else
                cmd.Parameters.AddWithValue("tier_art_id", DBNull.Value);
            if (t.Gehege_id.HasValue)
                cmd.Parameters.AddWithValue("gehege_id", t.Gehege_id);
            else
                cmd.Parameters.AddWithValue("gehege_id", DBNull.Value);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Tier_update(Tiere t, int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE tiere SET name = @name, gewicht = @gewicht, geburtsdatum = @geburtsdatum, tier_art_id = @tier_art_id, gehege_id = @gehege_id WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("name", t.Name);
            cmd.Parameters.AddWithValue("gewicht", t.Gewicht);
            cmd.Parameters.AddWithValue("geburtsdatum", t.Geburtstag);
            if (t.Tierart_id.HasValue)
                cmd.Parameters.AddWithValue("tier_art_id", t.Tierart_id);
            else
                cmd.Parameters.AddWithValue("tier_art_id", DBNull.Value);
            if (t.Gehege_id.HasValue)
                cmd.Parameters.AddWithValue("gehege_id", t.Gehege_id);
            else
                cmd.Parameters.AddWithValue("gehege_id", DBNull.Value);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein fehler ist aufgetreten: {ex.Message}");
            }
        }

        public void Tier_delete(int id)
        {

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM tiere WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ein fehler ist aufgetreten: {ex.Message}");
            }
        }

        private bool Prefix_Check(string s)
        {
            string prefix = "SELECT";

            return s.StartsWith(prefix);
        }

        public bool Select_Anfrage(string anfrage, out string ausgabe)
        {
            if (!Prefix_Check(anfrage))
            {
                ausgabe = "SELECT anfrage nicht erkannt";
                return false;
            }

            // user mit nur SELECT rechte
            using var conn = new NpgsqlConnection($"Host=localhost;Username=anfrage;Password=anfrage;Database=zooverwaltung");
            conn.Open();
            using var cmd = new NpgsqlCommand(anfrage, conn);

            var sb = new StringBuilder();
            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    int columnCount = reader.FieldCount;
                    //Console.WriteLine($"Columns: {columnCount}");

                    while (reader.Read())
                    {
                        for (int i = 0; i < columnCount; ++i)
                        {
                            if (reader.IsDBNull(i))
                                sb.Append("null");
                            else
                                sb.Append(reader.GetValue(i).ToString());
                            if (i != columnCount - 1)
                                sb.Append(",");

                        }
                        sb.AppendLine();
                    }
                }

                ausgabe = sb.ToString();
                return true;
            }
            catch (Exception ex)
            {
                ausgabe = $"Felher: {ex}";
                return false;
            }

        }
    }
}
