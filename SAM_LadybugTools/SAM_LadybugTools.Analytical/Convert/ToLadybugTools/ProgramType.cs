using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ProgramType ToLadybugTools(this Space space, AdjacencyCluster adjacencyCluster, ProfileLibrary profileLibrary)
        {
            InternalCondition internalCondition = space.InternalCondition;
            
            if (internalCondition == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(internalCondition);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            People people = null;
            Lighting lighting = null;
            ElectricEquipment electricEquipment = null;
            Infiltration infiltration = null;
            Setpoint setpoint = null;

            if (profileLibrary != null)
            {
                double area = double.NaN;
                if (!space.TryGetValue(SpaceParameter.Area, out area))
                    area = double.NaN;

                Dictionary<ProfileType, Profile> dictionary = internalCondition.GetProfileDictionary(profileLibrary);

                if (dictionary.ContainsKey(ProfileType.Occupancy))
                {
                    Profile profile = dictionary[ProfileType.Occupancy];
                    if (profile != null)
                    {
                        double gain = Analytical.Query.OccupancyGain(space);
                        if (double.IsNaN(gain))
                            gain = 0;
                        
                        ScheduleFixedInterval scheduleFixedInterval = profile.ToLadybugTools(ProfileType.Occupancy);
                        if(scheduleFixedInterval != null)
                        {
                            double gainPerPeople = gain;
                            if (double.IsNaN(gainPerPeople))
                                gainPerPeople = 0;

                            double occupancy = Analytical.Query.CalculatedOccupancy(space);
                            if (!double.IsNaN(occupancy) && occupancy != 0)
                                gainPerPeople = gainPerPeople / occupancy;

                            ScheduleFixedInterval scheduleFixedInterval_ActivityLevel = profile.ToLadybugTools_ActivityLevel(gainPerPeople);
                            if(scheduleFixedInterval_ActivityLevel != null)
                            {
                                double peoplePerArea = Analytical.Query.CalculatedPeoplePerArea(space);
                                if (double.IsNaN(peoplePerArea))
                                    peoplePerArea = 0;

                                double latentFraction = double.NaN;
                                double sensibleOccupancyGain = Analytical.Query.OccupancySensibleGain(space);
                                double latentOccupancyGain = Analytical.Query.OccupancyLatentGain(space);
                                if(!double.IsNaN(sensibleOccupancyGain) || !double.IsNaN(latentOccupancyGain))
                                    latentFraction = latentOccupancyGain / (latentOccupancyGain + sensibleOccupancyGain);

                                if (double.IsNaN(latentFraction))
                                    latentFraction = 0;

                                people = new People(string.Format("{0}_People", uniqueName), peoplePerArea, scheduleFixedInterval, scheduleFixedInterval_ActivityLevel, profile.Name, latentFraction: latentFraction);
                            }
                        }
                    }
                }

                if (dictionary.ContainsKey(ProfileType.Lighting))
                {
                    Profile profile = dictionary[ProfileType.Lighting];
                    if (profile != null)
                    {
                        double gain = Analytical.Query.CalculatedLightingGain(space);
                        if (double.IsNaN(gain))
                            gain = 0;

                        ScheduleFixedInterval scheduleFixedInterval = profile.ToLadybugTools(ProfileType.Lighting);
                        if (scheduleFixedInterval != null)
                        {
                            double gainPerArea = gain;
                            if (double.IsNaN(gainPerArea))
                                gainPerArea = 0;

                            if (!double.IsNaN(area) && area != 0)
                                gainPerArea = gainPerArea / area;
                            
                            lighting = new Lighting(string.Format("{0}_Lighting", uniqueName), gainPerArea, scheduleFixedInterval, profile.Name);
                        }
                    }
                }

                if (dictionary.ContainsKey(ProfileType.EquipmentSensible))
                {
                    double gain = Analytical.Query.CalculatedEquipmentSensibleGain(space);
                    if (double.IsNaN(gain))
                        gain = 0;

                    Profile profile = dictionary[ProfileType.EquipmentSensible];
                    if (profile != null)
                    {
                        ScheduleFixedInterval scheduleFixedInterval = profile.ToLadybugTools(ProfileType.EquipmentSensible);
                        if (scheduleFixedInterval != null)
                        {
                            double gainPerArea = gain;
                            if (double.IsNaN(gainPerArea))
                                gainPerArea = 0;

                            if (!double.IsNaN(area) && area != 0)
                                gainPerArea = gainPerArea / area;

                            electricEquipment = new ElectricEquipment(string.Format("{0}_ElectricEquipment", uniqueName), gainPerArea, scheduleFixedInterval, profile.Name);
                        }
                    }
                }

                if(dictionary.ContainsKey(ProfileType.Infiltration))
                {
                    Profile profile = dictionary[ProfileType.Infiltration];
                    if (profile != null)
                    {
                        ScheduleFixedInterval scheduleFixedInterval = profile.ToLadybugTools(ProfileType.Infiltration);
                        if (scheduleFixedInterval != null)
                        {
                            double airFlowPerExteriorArea = Query.InfiltrationAirFlowPerExteriorArea(adjacencyCluster, space);
                            
                            infiltration = new Infiltration(string.Format("{0}_Infiltration", uniqueName), airFlowPerExteriorArea, scheduleFixedInterval, profile.Name);
                        }
                    }
                }

                if(dictionary.ContainsKey(ProfileType.Cooling) && dictionary.ContainsKey(ProfileType.Heating))
                {
                    Profile profile_Cooling = dictionary[ProfileType.Cooling];
                    Profile profile_Heating = dictionary[ProfileType.Heating];
                    if(profile_Cooling != null && profile_Heating != null)
                    {
                        ScheduleFixedInterval scheduleFixedInterval_Cooling = profile_Cooling.ToLadybugTools(ProfileType.Cooling);
                        ScheduleFixedInterval scheduleFixedInterval_Heating = profile_Heating.ToLadybugTools(ProfileType.Heating);
                        if(scheduleFixedInterval_Cooling != null && scheduleFixedInterval_Heating != null)
                        {
                            //setpoint = new Setpoint(string.Format("{0}_Setpoint", uniqueName), scheduleFixedInterval_Cooling, scheduleFixedInterval_Heating, string.Format("Heating {0} Cooling {1}", profile_Heating.Name, profile_Cooling.Name));

                            ScheduleFixedInterval scheduleFixedInterval_Humidification = null;
                            if (dictionary.ContainsKey(ProfileType.Humidification))
                                scheduleFixedInterval_Humidification = dictionary[ProfileType.Humidification]?.ToLadybugTools(ProfileType.Humidification);

                            ScheduleFixedInterval scheduleFixedInterval_Dehumidification = null;
                            if (dictionary.ContainsKey(ProfileType.Dehumidification))
                                scheduleFixedInterval_Dehumidification = dictionary[ProfileType.Dehumidification]?.ToLadybugTools(ProfileType.Dehumidification);

                            setpoint = new Setpoint(string.Format("{0}_Setpoint", uniqueName), scheduleFixedInterval_Cooling, scheduleFixedInterval_Heating, string.Format("Heating {0} Cooling {1}", profile_Heating.Name, profile_Cooling.Name));//, scheduleFixedInterval_Humidification, scheduleFixedInterval_Dehumidification);
                            
                            if (scheduleFixedInterval_Dehumidification != null)
                                setpoint.DehumidifyingSchedule = scheduleFixedInterval_Dehumidification;
                            
                            if (scheduleFixedInterval_Humidification != null)
                                setpoint.HumidifyingSchedule = scheduleFixedInterval_Humidification;
                        }
                    }
                }
            }

            ProgramType result = new ProgramType(uniqueName, internalCondition.Name, people, lighting, electricEquipment, infiltration: infiltration, setpoint: setpoint);
            return result;
        }
    }
}