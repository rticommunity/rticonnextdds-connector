/*
 * Copyright (c) 2005-2017 Real-Time Innovations, Inc. All rights reserved.
 * Permission to modify and use for internal purposes granted.
 * This software is provided "as is", without warranty, express or implied.
 */

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include "lua_binding/lua_binding_ddsConnector.h"

#define MAX_ITERATIONS 10

/*
 * This function simulates a temperature sensor.
 * For demo purpose, it returns two values:
 * One of them will triggere the lua script to perform a write
 */
double getTemperature()
{
    if (rand()%2 == 0) {
        return 50;
    } else {
        return 82;
    }
}


/*
 * This function represents the main loop of a simple c application
 * After setting up the connector, it enters a loop
 * and, in each iteration, it:
 * - gets the temperature
 * - pushes it to the lua layer
 * - executes the lua code
 */
int main(int argc, char *argv[])
{
    int count = 0;
    double temp = 0.0;
    /*
     * The connector variable will be used to store a reference
     * to the connector
     */
    struct RTIDDSConnector *connector = NULL;

    /*
     * We create the connector by calling the RTIDDSConnector_new operation.
     * The first parameter is the Participant that has to be used
     * The second parameter is the XML file containing the configuration
     * The third parameter represent cofiguration to the Connector. It can be
     * NULL to use defaults.
     */
    connector = RTIDDSConnector_new(
            "MyParticipantLibrary::Zero",
            "./Simple.xml", NULL);
    if (connector == NULL) {
        printf("Failed to create connector\n");
        return -1;
    }

    /*
     * With the call to RTIDDSConnector_assertCode we are telling the connector
     * what scripts has to be executed when RTIDDSConnector_execute is called.
     * The first parameter is the connector itself
     * The second paramenter can be a string representing a lua scripts or NULL
     * The third parameter is the path to a file containing a lua script.
     * The last paramenter is the interval in seconds at which the lua script
     * is checked for changes. A negative value disable reloads.
     */
    RTIDDSConnector_assertCode(connector,NULL,"./Alert.lua",4);

    for (count=0;count<MAX_ITERATIONS;count++) {
        /*
         * Every 4 seconds...
         */
        sleep(4);
        /*
         * ... we obtain a temperature value...
         */
        temp = getTemperature();

        /*
         * ... we store it into the lua layer...
         */
        if (!RTIDDSConnector_setNumberIntoContext(connector,"temp", temp)) {
            printf("Failed to assertNumber\n");
        }

        /*
         * ... and we invoke the lua script.
         */
        if (RTIDDSConnector_execute(connector)!=0) {
            printf("Error executing lua\n");
            return -1;
        }
    }
    /*
     * Delete the connector and exit.
     */
    RTIDDSConnector_delete(connector);
    return 0;
}
