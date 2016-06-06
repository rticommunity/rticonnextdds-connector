import sys
import os
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)),os.pardir))
import rticonnextdds_connector as rti
import pytest

class TestConnector:
  # TODO:  rti.Connector should raise an exception when xml file path is invalid
  def test_invalid_xml_path(self,capfd):
    participant_profile = "MyParticipantLibrary::Zero"
    invalid_xml_path = "invalid/path/to/xml"
    connector = rti.Connector(participant_profile,invalid_xml_path)
    out,err = capfd.readouterr()
    assert "DDS_QosProvider_load_profiles_from_url_groupI:ERROR" in out 
  
  # TODO: rti.Connector should raise an exception when Participant profile does not exist
  def test_invalid_participant_profile(self,capfd):
    invalid_participant_profile = "InvalidParticipantProfile"  
    xml_path = os.path.join(os.path.dirname(os.path.realpath(__file__)),
      "../examples/python/ShapeExample.xml")
    connector = rti.Connector(invalid_participant_profile,xml_path)
    out,err = capfd.readouterr()
    assert "DDS_DomainParticipantFactory_create_participant_from_config_w_paramsI:ERROR" in out 
  
  # TODO: rti.Connector should raise an exception if xml file contents have an error
  def test_ivalid_xml_profile(self,capfd):
    participant_profile = "MyParticipantLibrary::Zero"
    invalid_xml = os.path.join(os.path.dirname(os.path.realpath(__file__)),
      "../examples/python/InvalidXml.xml")

    connector = rti.Connector(participant_profile,invalid_xml)
    out,err = capfd.readouterr()
    assert "DDS_XMLParser_parse_from_file:Error" in out 

class TestOutput:

  # TODO: Output object should not get created if DW name does not exist in XML
  # All methods: setNumber, setBoolean,  setString, setDictionary and write fail
  @pytest.mark.xfail
  def test_invalid_DW(self,connector):
    invalid_DW = "InvalidDW"
    op= connector.getOutput(invalidDW)
    assert op== None
    
  def test_setNumber_on_nonexistant_field(self,connector,capfd):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    non_existant_field="invalid_field"
    op.instance.setNumber(non_existant_field,1)
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  def test_setString_on_nonexistant_field(self,connector,capfd):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    non_existant_field="invalid_field"
    op.instance.setString(non_existant_field,"1")
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  def test_setBoolean_on_nonexistant_field(self,connector,capfd):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    non_existant_field="invalid_field"
    op.instance.setBoolean(non_existant_field,True)
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  def test_setDictionary_with_nonexistant_fields(self,connector,capfd):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    invalid_dictionary= {"non_existant_field":"value"}
    op.instance.setDictionary(invalid_dictionary)
    out,err = capfd.readouterr()
    assert "RTILuaJsonHelper_parse_json_node:!get kind failed" in out

  @pytest.mark.xfail
  def test_setNumber_with_Boolean_value(self,connector):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    number_field="x"
    with pytest.raises(Exception) as execinfo:
      op.instance.setNumber(number_field,True)
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("Traceback:"+str(execinfo.traceback))

  def test_setNumber_with_String_value(self,connector):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    number_field="x"
    with pytest.raises(Exception) as execinfo:
      op.instance.setNumber(number_field,"str")
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("\nTraceback:"+str(execinfo.traceback))
 
  @pytest.mark.xfail
  def test_setNumber_with_Float_value(self,connector):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    number_field="x"
    with pytest.raises(Exception) as execinfo:
      op.instance.setNumber(number_field,55.55)
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("\nTraceback:"+str(execinfo.traceback))

  def test_setString_with_Boolean_value(self,connector):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    string_field="color"
    with pytest.raises(Exception) as execinfo:
      op.instance.setString(string_field,True)
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("Traceback:"+str(execinfo.traceback))

  def test_setString_with_Number_value(self,connector):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    string_field="color"
    with pytest.raises(Exception) as execinfo:
      op.instance.setString(string_field,55.55)
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("\nTraceback:"+str(execinfo.traceback))
  
  # TODO: implement tests for setBoolean for types String,Number

  # TODO: A dictionary with incompatible types can be set 
  def test_setDictionary_with_incompatible_types(self,connector,capfd):
    valid_DW = "MyPublisher::MySquareWriter"
    op= connector.getOutput(valid_DW)
    dict_with_incompatible_types={"color":1,"x":"str"}
    op.instance.setDictionary(dict_with_incompatible_types)
    out,err = capfd.readouterr()

class TestInput:

  # TODO: Input object should not get created if DR name does not exist in XML
  """
  All functions like take(),read() will fail except for wait()
  All functions on input.samples: getLength,getNumber,getString,
      getBoolean and getDictionary will fail
  All functions on input.infos: getLength and isValid will fail
  """
  @pytest.mark.xfail
  def test_invalid_DR(self,connector):
    invalid_DR = "InvalidDR"
    inp = connector.getInput(invalid_DR)
    assert op== None
  
  @pytest.mark.xfail
  def test_wait_on_invalid_DR(self,connector):
    invalid_DR = "InvalidDR"
    inp = connector.getInput(invalid_DR)
    with pytest.raises(Exception) as execinfo:
      inp.wait(1)

  #TODO: Address segmentation fault on out of index and 0-index access 
  """
  Segmentation fault occurs when 0-index or out-of-bound access is made 
  on infos and samples: 
  infos: isValid
  samples: getNumber, getBoolean, getString, getDictionary
  """
