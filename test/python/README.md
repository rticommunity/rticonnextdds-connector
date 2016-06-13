# Python Testing Documentation

To run the tests for python binding of rticonnextdds_connector:

1. Install [pytest](https://pytest.org/latest/contents.html) with:

  ``pip install pytest``

2. To execute all the tests, issue the following command from the base directory: 
  
   ``py.test ./test/python``
  
   To execute each test individually, include the name of the test file: 
  
   ``py.test ./test/python/test_rticonnextdds_input.py``

All the tests are documented in their respective source files following the [docstrings](https://www.python.org/dev/peps/pep-0257/)
python documentation convention.
