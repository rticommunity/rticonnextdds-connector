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

#class TestOutput:
#
#  # TODO: Output object should not get created if DW name does not exist in XML
#  """
#  All methods: setNumber, setBoolean,  setString, setDictionary on Instance and 
#  write fail
#  """
#  @pytest.mark.xfail
#  def test_invalid_DW(self,connector):
#    invalid_DW = "InvalidDW"
#    op= connector.getOutput(invalidDW)
#    assert op== None
#    
#  # TODO: No exception is thrown when a non-existent field is accessed
#  def test_setNumber_on_nonexistant_field(self,out,capfd):
#    non_existant_field="invalid_field"
#    out.instance.setNumber(non_existant_field,1)
#    out,err = capfd.readouterr()
#    assert "RTILuaDynamicData_set:!get kind failed" in out
#
#  def test_setString_on_nonexistant_field(self,out,capfd):
#    non_existant_field="invalid_field"
#    out.instance.setString(non_existant_field,"1")
#    out,err = capfd.readouterr()
#    assert "RTILuaDynamicData_set:!get kind failed" in out
#
#  def test_setBoolean_on_nonexistant_field(self,out,capfd):
#    non_existant_field="invalid_field"
#    out.instance.setBoolean(non_existant_field,True)
#    out,err = capfd.readouterr()
#    assert "RTILuaDynamicData_set:!get kind failed" in out
#
#  def test_setDictionary_with_nonexistant_fields(self,out,capfd):
#    invalid_dictionary= {"non_existant_field":"value"}
#    out.instance.setDictionary(invalid_dictionary)
#    out,err = capfd.readouterr()
#    assert "RTILuaJsonHelper_parse_json_node:!get kind failed" in out
#
#  # TODO: Should user be notified/warned about type incompatibility
#  @pytest.mark.xfail
#  def test_setNumber_with_Boolean_value(self,out):
#    number_field="x"
#    with pytest.raises(Exception) as execinfo:
#      out.instance.setNumber(number_field,True)
#    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
#    print("Traceback:"+str(execinfo.traceback))
#
#  # TODO: ctypes exception is not propagated via the connector
#  def test_setNumber_with_String_value(self,out):
#    number_field="x"
#    with pytest.raises(Exception) as execinfo:
#      out.instance.setNumber(number_field,"str")
#    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
#    print("\nTraceback:"+str(execinfo.traceback))
# 
#  # TODO: should user be warned? 
#  @pytest.mark.xfail
#  def test_setNumber_with_Float_value(self,out):
#    number_field="x"
#    with pytest.raises(Exception) as execinfo:
#      out.instance.setNumber(number_field,55.55)
#    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
#    print("\nTraceback:"+str(execinfo.traceback))
#
#  def test_setString_with_Boolean_value(self,out):
#    string_field="color"
#    with pytest.raises(Exception) as execinfo:
#      out.instance.setString(string_field,True)
#    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
#    print("Traceback:"+str(execinfo.traceback))
#
#  def test_setString_with_Number_value(self,out):
#    string_field="color"
#    with pytest.raises(Exception) as execinfo:
#      out.instance.setString(string_field,55.55)
#    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
#    print("\nTraceback:"+str(execinfo.traceback))
#  
#  # TODO: implement tests for setBoolean for types String,Number
#
#  # TODO: A dictionary with incompatible types can be set!!!
#  def test_setDictionary_with_incompatible_types(self,out,capfd):
#    dict_with_incompatible_types={"color":1,"x":"str"}
#    out.instance.setDictionary(dict_with_incompatible_types)
#    out,err = capfd.readouterr()
#    assert 0
#
#class TestInput:
#
#  # TODO: Input object should not get created if DR name does not exist in XML
#  """
#  All functions like take(),read() will fail except for wait()
#  All functions on input.samples: getLength,getNumber,getString,
#      getBoolean and getDictionary will fail
#  All functions on input.infos: getLength and isValid will fail
#  """
#  @pytest.mark.xfail
#  def test_invalid_DR(self,connector):
#    invalid_DR = "InvalidDR"
#    inp = connector.getInput(invalid_DR)
#    assert inp== None
#  
#  # TODO: figure out how test the wait function
#  @pytest.mark.xfail
#  def test_wait_on_invalid_DR(self,connector):
#    invalid_DR = "InvalidDR"
#    inp = connector.getInput(invalid_DR)
#    with pytest.raises(Exception) as execinfo:
#      inp.wait(1)
#
#  #TODO: Address segmentation fault on out of index and 0-index access 
#  """
#  Segmentation fault occurs when 0-index or out-of-bound access is made 
#  on infos and samples: 
#  infos: isValid
#  samples: getNumber, getBoolean, getString, getDictionary
#  """
#
#class TestDataFlow:
#  def test_dataflow_with_read(self,inp,out):
#    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
#    out.instance.setDictionary(msg)
#    out.write()
#    received_count=0
#    received_msg={}
#    for i in range(1,20):
#	time.sleep(.5)
#        inp.read()
#        numOfSamples  = inp.samples.getLength()
#	if numOfSamples > 0:
#          received_count+= numOfSamples
#          for j in range(1, numOfSamples+1):
#              if inp.infos.isValid(j):
#                received_msg = inp.samples.getDictionary(j)
#        if received_count==1:
#          break
#    assert received_count==1 and received_msg==msg
#
#  # TODO: Figure out how to take data on the basis of read/seen status
#  def test_dataflow_with_take(self,inp,out):
#    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
#    out.instance.setDictionary(msg)
#    out.write()
#    received_count=0
#    received_msg={}
#    for i in range(1,20):
#	time.sleep(.5)
#        inp.take()
#        numOfSamples  = inp.samples.getLength()
#	if numOfSamples > 0:
#          received_count+= numOfSamples
#          for j in range(1, numOfSamples+1):
#            if inp.infos.isValid(j):
#              x = inp.samples.getNumber(j,"x")
#              y = inp.samples.getNumber(j,"y")
#              color = inp.samples.getString(j,"color")
#              shapesize  = inp.samples.getNumber(j,"shapesize")
#              received_msg={"x":x,"y":y,"color":color,"shapesize":shapesize}
#        if received_count==1:
#          break
#    assert received_count==1 and received_msg==msg
#
#  # TODO: accessing non-existent field should throw an exception
#  def test_getNumber_for_nonexistent_field(self,inp,out,capfd):
#    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
#    out.instance.setDictionary(msg)
#    out.write()
#    received_count=0
#    for i in range(1,20):
#	time.sleep(.5)
#        inp.take()
#        numOfSamples  = inp.samples.getLength()
#	if numOfSamples > 0:
#          received_count+= numOfSamples
#          for j in range(1, numOfSamples+1):
#            if inp.infos.isValid(j):
#              m = inp.samples.getNumber(j,"m")
#        if received_count==1:
#          break
#    out,err=capfd.readouterr()
#    assert "DynamicData_get:!get kind failed" in out
#
#  def test_getString_for_nonexistent_field(self,inp,out,capfd):
#    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
#    out.instance.setDictionary(msg)
#    out.write()
#    received_count=0
#    for i in range(1,20):
#	time.sleep(.5)
#        inp.take()
#        numOfSamples  = inp.samples.getLength()
#	if numOfSamples > 0:
#          received_count+= numOfSamples
#          for j in range(1, numOfSamples+1):
#            if inp.infos.isValid(j):
#              m = inp.samples.getString(j,"m")
#        if received_count==1:
#          break
#    out,err=capfd.readouterr()
#    assert "DynamicData_get:!get kind failed" in out
#
#  def test_getBoolean_for_nonexistent_field(self,inp,out,capfd):
#    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
#    out.instance.setDictionary(msg)
#    out.write()
#    received_count=0
#    for i in range(1,20):
#	time.sleep(.5)
#        inp.take()
#        numOfSamples  = inp.samples.getLength()
#	if numOfSamples > 0:
#          received_count+= numOfSamples
#          for j in range(1, numOfSamples+1):
#            if inp.infos.isValid(j):
#              m = inp.samples.getBoolean(j,"m")
#        if received_count==1:
#          break
#    out,err=capfd.readouterr()
#    assert "DynamicData_get:!get kind failed" in out
#  
#  # TODO: tests for invalid type access for methods getNumber,getString & getBoolean
#  """
#  getString on numeric field gives a string representation of the number. 
#  getString on boolean field gives string representation of True/False
# 
#  getBoolean on string or numeric field gives 0/1
#  
#  getNumber on a boolean field gives 0/1 
#  getNumber on string field gives a random number
#  """
