using UnityEngine;

public static class DistrictArray
{
    public static District[] DisrictArr;
    private static int DistrictNumber = 27;

    public static District[] GetAllDistricts()
    {
        if (DisrictArr == null)
        {
            SetAllDistricts();
        }
        return DisrictArr;
    }

    public static District GetDistrict(int _index)
    {
        if (DisrictArr == null)
        {
            SetAllDistricts();
        }

        if (Board.isMultiplayer)
        {
            return DisrictArr[0];
        }
        return DisrictArr[_index];
    }

    /// <summary>
    /// Einwohnerzahlen von Statistik Austria, Stand: Jänner 2019
    /// Photovoltaik / 1000 Einwohner Statistik Austria (ATLAS), Stand: Oktober 2019
    /// </summary>
    private static void SetAllDistricts()
    {
        DisrictArr = new District[DistrictNumber];
        District badZell = new District("Bad Zell", 48.3496, 14.6686, 2913, 20.55, 45.52);
        District freistadt = new District("Freistadt", 48.5113, 14.5048, 7960, 12.39, 12.86);
        District gruenbach = new District("Grünbach", 48.5365, 14.5365, 1924, 13.12, 36.08);
        District gutau = new District("Gutau", 48.4184, 14.6122, 2672, 22.39, 45.28);
        District hagenberg = new District("Hagenberg", 48.3674, 14.5169, 2751, 12.06, 15.05);
        District hirschbach = new District("Hirschbach", 48.4883, 14.4113, 1202, 29.77, 23.63);
        District kaltenberg = new District("Kaltenberg", 46.7722, 14.5671, 622, 24.00, 17.20);
        District kefermarkt = new District("Kefermarkt", 48.443, 14.5384, 2138, 14.18, 27.81);
        District koenigswiesen = new District("Königswiesen", 48.4057, 14.8388, 3091, 11.56, 73.38);
        District lasberg = new District("Lasberg", 48.4714, 14.5408, 2796, 16.01, 43.80);
        District leopoldschlag = new District("Leopoldschlag", 48.6159, 14.5036, 1015, 29.77, 25.80);
        District liebenau = new District("Liebenau", 48.532, 14.8052, 1585, 10.64, 76.31);
        District neumarkt = new District("Neumarkt", 48.4283, 14.4845, 3163, 14.23, 46.67);
        District pierbach = new District("Pierbach", 48.3482, 14.7565, 1016, 29.77, 22.72);
        District pregarten = new District("Pregarten", 48.355, 14.5308, 5422, 16.81, 27.77);
        District rainbach = new District("Rainbach", 48.5576, 14.4765, 2989, 13.86, 49.27);
        District sandl = new District("Sandl", 48.5604, 14.6444, 1413, 20.95, 58.32);
        District schoenau = new District("Schönau", 48.3942, 14.7291, 1949, 28.34, 38.54);
        District sanktLeonhard = new District("St. Leonhard", 48.4436, 14.6776, 1388, 23.67, 35.01);
        District sanktOswald = new District("St. Oswald", 48.5006, 14.5877, 2906, 16.89, 40.98);
        District tragwein = new District("Tragwein", 48.332, 14.6212, 3099, 21.09, 39.47);
        District unterweissenbach = new District("Unterweißenbach", 46.9516, 15.8625, 2174, 21.44, 48.73);
        District unterweitersdorf = new District("Unterweitersdorf", 48.3673, 14.4689, 2161, 19.21, 11.42);
        District waldburg = new District("Waldburg", 48.509, 14.439, 1382, 15.27, 26.53);
        District wartberg = new District("Wartberg ob der Aist", 48.3503, 14.5082, 4276, 18.00, 19.41);
        District weitersfelden = new District("Weitersfelden", 48.4779, 14.7265, 1047, 19.19, 43.72);
        District windhaag = new District("Windhaag", 48.5862, 14.5615, 1567, 29.77, 42.83);

        DisrictArr[0] = freistadt;
        DisrictArr[1] = pregarten;
        DisrictArr[2] = wartberg;
        DisrictArr[3] = neumarkt;
        DisrictArr[4] = tragwein;
        DisrictArr[5] = koenigswiesen;
        DisrictArr[6] = rainbach;
        DisrictArr[7] = badZell;
        DisrictArr[8] = sanktOswald;
        DisrictArr[9] = lasberg;
        DisrictArr[10] = hagenberg;
        DisrictArr[11] = gutau;
        DisrictArr[12] = unterweissenbach;
        DisrictArr[13] = unterweitersdorf;
        DisrictArr[14] = kefermarkt;
        DisrictArr[15] = schoenau;
        DisrictArr[16] = gruenbach;
        DisrictArr[17] = liebenau;
        DisrictArr[18] = windhaag;
        DisrictArr[19] = sandl;
        DisrictArr[20] = sanktLeonhard;
        DisrictArr[21] = waldburg;
        DisrictArr[22] = hirschbach;
        DisrictArr[23] = weitersfelden;
        DisrictArr[24] = pierbach;
        DisrictArr[25] = leopoldschlag;
        DisrictArr[26] = kaltenberg;
    }
}
