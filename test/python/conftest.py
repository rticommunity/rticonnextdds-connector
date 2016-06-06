import sys,os,pytest
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)),os.pardir))
import rticonnextdds_connector as rti

@pytest.fixture(scope="session")
def connector(request):
  xml_path= os.path.join(os.path.dirname(os.path.realpath(__file__)),
    "../examples/python/ShapeExample.xml") 
  participant_profile="MyParticipantLibrary::Zero"
  rti_connector = rti.Connector(participant_profile,xml_path)
  def cleanup():
    #TODO implement function to cleanup rti.Connector
    print("\n Cleanup function called for connector")
  request.addfinalizer(cleanup)
  return rti_connector
