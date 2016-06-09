import pytest
class TestInput:

  # TODO: Input object should not get created if DR name does not exist in XML
  """
  All functions like take(),read() will fail except for wait()
  All functions on input.samples: getLength,getNumber,getString,
      getBoolean and getDictionary will fail
  All functions on input.infos: getLength and isValid will fail
  """

  def test_invalid_DR(self,connector):
    invalid_DR = "InvalidDR"
    with pytest.raises(ValueError):
       connector.getInput(invalid_DR)
  
  # TODO: figure out how test the wait function
#  @pytest.mark.xfail
#  def test_wait_on_invalid_DR(self,connector):
#    invalid_DR = "InvalidDR"
#    inp = connector.getInput(invalid_DR)
#    with pytest.raises(Exception) as execinfo:
#      inp.wait(1)
#
  #TODO: Address segmentation fault on out of index and 0-index access 
  """
  Segmentation fault occurs when 0-index or out-of-bound access is made 
  on infos and samples: 
  infos: isValid
  samples: getNumber, getBoolean, getString, getDictionary
  """
