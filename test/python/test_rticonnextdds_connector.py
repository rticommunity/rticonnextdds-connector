import sys,time,os,pytest
sys.path.append(os.path.dirname(os.path.realpath(__file__))+ "/../../")
import rticonnextdds_connector as rti

class TestConnector:

  def test_invalid_xml_path(self):
    participant_profile = "MyParticipantLibrary::Zero"
    invalid_xml_path = "invalid/path/to/xml"
    with pytest.raises(ValueError):
      connector = rti.Connector(participant_profile,invalid_xml_path)
  
  def test_invalid_participant_profile(self):
    invalid_participant_profile = "InvalidParticipantProfile"  
    xml_path = os.path.join(os.path.dirname(os.path.realpath(__file__)),
      "../xml/ShapeExample.xml")
    with pytest.raises(ValueError):
      connector = rti.Connector(invalid_participant_profile,xml_path)
  
  def test_ivalid_xml_profile(self):
    participant_profile = "MyParticipantLibrary::Zero"
    invalid_xml = os.path.join(os.path.dirname(os.path.realpath(__file__)),
      "../xml/InvalidXml.xml")
    with pytest.raises(ValueError):
      connector = rti.Connector(participant_profile,invalid_xml)
  
  def test_connector_creation(self,connector):
    assert connector!=None and isinstance(connector,rti.Connector)
