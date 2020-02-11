public class District
{
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Residents { get; set; }
    public double PVs { get; set; }
    public double Area { get; set; }

    public District(string _name, double _lat, double _lon, int _res, double _pvsPer1000, double _area)
    {
        Name = _name;
        Latitude = _lat;
        Longitude = _lon;
        Residents = _res;
        PVs = CalculatePVs(_pvsPer1000, _res);
        Area = _area;
    }

    private double CalculatePVs(double _pvsPer1000, int _res)
    {
        double pvPerSingleResident = _pvsPer1000 / 1000;
        return pvPerSingleResident * _res;
    }

}
