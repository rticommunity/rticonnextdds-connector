rticonnextdds-connector
=======================
This Git repository contains all the files you need to start using the 
RTI Connext DDS Connector in [Node.js](https://nodejs.org/).

With the sole exception of the contents of the "examples" subdirectory, all use
of this product is subject to the RTI Software License Agreement included at 
the top level of this repository.  Files within the "examples" subdirectory are
licensed as marked within the file.

This software is an experimental (aka "pre-production") product. The Software is
provided "as is", with no warranty of any type, including any warranty for
fitness for any purpose. RTI is under no obligation to maintain or
support the Software. RTI shall not be liable for any incidental or
consequential  damages arising out of the use or inability to use the software.

You can ask questions and find help on http://community.rti.com

HowTo
=====
    git clone git@github.com:rticommunity/rticonnextdds-connector.git

    cd rticonnextdds-connector
    npm install

    cd examples/nodejs/
    export NODE_PATH=../..

    node web_socket/reader_websocket.js 
    Point browser to http://127.0.0.1:7400/

    Start Shapes Demo and publish Squares OR in another shell window
    node simple/writer.js
    
    The web-browser display should update to show the published squares
