﻿using Model;
using RollingStock;
using RouteManager.v2.dataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = RouteManager.v2.Logging.Logger;

namespace RouteManager.v2.core
{
    public static class StationManager
    {
        //Determine if station is selected
        public static bool IsStationSelected(PassengerStop stop, Car locomotive)
        {
            //Trace Function
            Logger.LogToDebug("ENTERED FUNCTION: IsStationSelected", Logger.logLevel.Trace);

            bool result =  LocoTelem.UIStationSelections[locomotive].TryGetValue(stop.identifier, out bool isSelected) && isSelected;

            //Trace Function
            Logger.LogToDebug("EXITING FUNCTION: IsStationSelected", Logger.logLevel.Trace);
            return result; 
        }

        //Update station selection
        public static void SetStationSelected(PassengerStop stop, Car locomotive, bool isSelected)
        {
            //Trace Function
            Logger.LogToDebug("ENTERED FUNCTION: SetStationSelected", Logger.logLevel.Trace);

            LocoTelem.UIStationSelections[locomotive][stop.identifier] = isSelected;

            //Trace Function
            Logger.LogToDebug("EXITING FUNCTION: SetStationSelected", Logger.logLevel.Trace);
        }

        //Check if Consist has any stations enabled
        public static bool IsAnyStationSelectedForLocomotive(Car locomotive)
        {
            //Trace Function
            Logger.LogToDebug("ENTERED FUNCTION: IsAnyStationSelectedForLocomotive", Logger.logLevel.Trace);

            // Check if the locomotive exists in the SelectedStations dictionary
            if (LocoTelem.SelectedStations.TryGetValue(locomotive, out List<PassengerStop> selectedStations))
            {
                // Return true if there is at least one selected station
                return selectedStations.Any();
            }

            //Trace Function
            Logger.LogToDebug("EXITING FUNCTION: IsAnyStationSelectedForLocomotive", Logger.logLevel.Trace);

            // Return false if the locomotive is not found or no stations are selected
            return false;
        }

        //Setup create station selection list of current consist.
        public static void InitializeStationSelectionForLocomotive(Car locomotive)
        {
            //Trace Function
            Logger.LogToDebug("ENTERED FUNCTION: InitializeStationSelectionForLocomotive", Logger.logLevel.Trace);

            if (!LocoTelem.UIStationSelections.ContainsKey(locomotive))
            {
                var stationSelectionsForLocomotive = new Dictionary<string, bool>();
                var allStops = PassengerStop.FindAll();

                foreach (var stop in allStops)
                {
                    stationSelectionsForLocomotive[stop.identifier] = false;
                }

                LocoTelem.UIStationSelections[locomotive] = stationSelectionsForLocomotive;
            }

            //Trace Function
            Logger.LogToDebug("EXITING FUNCTION: InitializeStationSelectionForLocomotive", Logger.logLevel.Trace);
        }
    }
}