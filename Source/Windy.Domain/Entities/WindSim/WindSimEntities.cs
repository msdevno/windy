using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Windy.Domain.Entities.WindSim
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PowerForecastData
    {
        private Dictionary<string, Dictionary<DateTime, ForecastElementInfo>> _powerForecast = new Dictionary<string, Dictionary<DateTime, ForecastElementInfo>>();

        public Dictionary<string, Dictionary<DateTime, ForecastElementInfo>> PowerForecast
        {
            get { return _powerForecast; }
            set { _powerForecast = value; }
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ForecastElementInfo
    {
        public ForecastElementType Type { get; private set; }

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                Type = value == "ALL" ? ForecastElementType.WindFarm : ForecastElementType.Turbine;
            }
        }

        public double PowerKW { get; set; }
    }


    /// <summary>
    /// Represents the ElementType of Forecast
    /// </summary>
    public enum ForecastElementType
    {
        WindFarm = 0,
        Turbine
    }
}
