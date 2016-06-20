# Nodejs Testing Documentation

To run the tests for nodejs binding of **rticonnextdds_connector**:

1. Install [chai](http://chaijs.com/) assertion library, [sinon](http://sinonjs.org/) for test spies and [mocha](https://mochajs.org/) testing framework with:

  ```
  npm install chai
  npm install mocha 
  npm install sinon 
  ```

2. To execute all the tests, issue the following command from the base directory: 
  
   ``mocha ./test/nodejs``
  
   To execute each test individually, also include the name of the test file: 
  
   ``mocha ./test/nodejs/test_rticonnextdds_dataflow.js``

**Note:** Some tests are marked to be skipped because the functionality being tested is not yet supported by the nodejs connector library. These tests will be reported as ``pending``.


Nodejs tests are organized as follows:

1. ``test_rticonnextdds_connector.js``: Contains tests for ``rticonnextdds_connector.Connector`` object
2. ``test_rticonnextdds_input.js``: Contains tests for ``rticonnextdds_connector.Input`` object
3. ``test_rticonnextdds_output.js``: Contains tests for ``rticonnextdds_connector.Output`` object
4. ``test_rticonnextdds_dataflow.js``: Tests the dataflow between an ``rticonnextdds_connector.Input`` and ``rticonnextdds_connector.Output`` object.
