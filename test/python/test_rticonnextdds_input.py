import pytest,sys,os
sys.path.append(os.path.dirname(os.path.realpath(__file__))+ "/../../")
import rticonnextdds_connector as rti

class TestInput:

  def test_invalid_DR(self,rtiConnectorFixture):
    invalid_DR = "InvalidDR"
    with pytest.raises(ValueError):
       rtiConnectorFixture.getInput(invalid_DR)
  
  def test_creation_DR(self,rtiInputFixture):
    assert rtiInputFixture!=None and isinstance(rtiInputFixture,rti.Input) \
      and rtiInputFixture.name == "MySubscriber::MySquareReader" \
      and isinstance(rtiInputFixture.connector,rti.Connector) \
      and isinstance(rtiInputFixture.samples,rti.Samples) \
      and isinstance(rtiInputFixture.infos,rti.Infos)

  # TODO: figure out how to test the wait function

  # TODO: Address segmentation fault on out of index and 0-index access 
  """
  Segmentation fault occurs when 0-index or out-of-bound access is made 
  on infos and samples: 
  infos: isValid
  samples: getNumber, getBoolean, getString, getDictionary
  """
