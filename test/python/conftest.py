import sys,os,pytest,threading,time
sys.path.append(os.path.dirname(os.path.realpath(__file__))+ "/../../")
import rticonnextdds_connector as rti

@pytest.fixture(scope="session")
def connector(request):
  xml_path= os.path.join(os.path.dirname(os.path.realpath(__file__)),
    "../xml/ShapeExample.xml") 
  participant_profile="MyParticipantLibrary::Zero"
  rti_connector = rti.Connector(participant_profile,xml_path)
  def cleanup():
    #TODO implement function to cleanup rti.Connector
    print("\n Cleanup function called for connector")
  request.addfinalizer(cleanup)

  return rti_connector

@pytest.fixture(scope="session")
def out(connector):
  DW="MyPublisher::MySquareWriter"
  return connector.getOutput(DW)

@pytest.fixture(scope="session")
def inp(connector):
  DR="MySubscriber::MySquareReader"
  return connector.getInput(DR)
