/*****************************************************************************
*    (c) 2005-2015 Copyright, Real-Time Innovations, All rights reserved.    *
*                                                                            *
* RTI grants Licensee a license to use, modify, compile, and create          *
* derivative works of the Software.  Licensee has the right to distribute    *
* object form only for use with RTI products. The Software is provided       *
* "as is", with no warranty of any type, including any warranty for fitness  *
* for any purpose. RTI is under no obligation to maintain or support the     *
* Software.  RTI shall not be liable for any incidental or consequential     *
* damages arising out of the use or inability to use the software.           *
*                                                                            *
******************************************************************************/

module.exports = function(RED) {
    "use strict";

    var rti = require("rticonnextdds-connector");


    // The main node definition - most things happen in here
    function RTIConnectorOutNode(n) {
        // Create a RED node
        RED.nodes.createNode(this,n);


        // Store local copies of the node configuration (as defined in the .html)
        this.profile_name = n.profile_name;
        this.xml = n.xml;
        this.output_name = n.output_name;

        // Do whatever you need to do in here - declare callbacks etc


        var node = this;

        var connector = new rti.Connector(this.profile_name,this.xml);
        var output = connector.getOutput(this.output_name);

        node.on("input", function(msg) {
            if (msg.payload != null) {
              output.instance.setFromJSON(JSON.parse(msg.payload));
              output.write();
            }
        });

        // send out the message to the rest of the workspace.

        node.on("close", function () {

            // Called when the node is shutdown - eg on redeploy.
            // Allows ports to be closed, connections dropped etc.
            // eg: this.client.disconnect();
        });
    }

    // Register the node by name. This must be called before overriding any of the
    // Node functions.
    RED.nodes.registerType("RTIConnectorOut",RTIConnectorOutNode);

}

