import pytest,sys,os
sys.path.append(os.path.dirname(os.path.realpath(__file__))+ "/../../")
import rticonnextdds_connector as rti

class TestOutput:

  def test_invalid_DW(self,rtiConnectorFixture):
    invalid_DW = "InvalidDW"
    with pytest.raises(ValueError):
      op= rtiConnectorFixture.getOutput(invalid_DW)
 
  def test_creation_DW(self,rtiOutputFixture):
    assert isinstance(rtiOutputFixture,rti.Output) \
      and rtiOutputFixture.name == "MyPublisher::MySquareWriter" \
      and isinstance(rtiOutputFixture.connector,rti.Connector) \
      and isinstance(rtiOutputFixture.instance, rti.Instance)
    
class TestInstance:

  def test_instance_creation(self,rtiOutputFixture):
    assert rtiOutputFixture.instance!=None and \
      isinstance(rtiOutputFixture.instance, rti.Instance)

  # TODO: No exception is thrown when a non-existent field is accessed
  """
  An AttributeError must be propagated on setNumber,setString & setBoolean
  KeyError must be propagated on setDictionary with invalid fields 
  """
  @pytest.mark.xfail
  def test_setNumber_on_nonexistent_field(self,rtiOutputFixture,capfd):
    non_existent_field="invalid_field"
    rtiOutputFixture.instance.setNumber(non_existent_field,1)
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  @pytest.mark.xfail
  def test_setString_on_nonexistent_field(self,rtiOutputFixture,capfd):
    non_existent_field="invalid_field"
    rtiOutputFixture.instance.setString(non_existent_field,"1")
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  @pytest.mark.xfail
  def test_setBoolean_on_nonexistent_field(self,rtiOutputFixture,capfd):
    non_existent_field="invalid_field"
    rtiOutputFixture.instance.setBoolean(non_existent_field,True)
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  @pytest.mark.xfail
  def test_setDictionary_with_nonexistent_fields(self,rtiOutputFixture,capfd):
    invalid_dictionary= {"non_existent_field":"value"}
    rtiOutputFixture.instance.setDictionary(invalid_dictionary)
    out,err = capfd.readouterr()
    assert "RTILuaJsonHelper_parse_json_node:!get kind failed" in out

  @pytest.mark.xfail
  # Implicit type conversion from Boolean to number 
  def test_setNumber_with_Boolean(self,rtiOutputFixture):
    number_field="x"
    with pytest.raises(TypeError) as execinfo:
      rtiOutputFixture.instance.setNumber(number_field,True)
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))

  def test_setNumber_with_String(self,rtiOutputFixture):
    number_field="x"
    with pytest.raises(TypeError) as execinfo:
      rtiOutputFixture.instance.setNumber(number_field,"str")
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))
 
  def test_setNumber_with_Dictionary(self,rtiOutputFixture):
    number_field="x"
    with pytest.raises(TypeError) as execinfo:
      rtiOutputFixture.instance.setNumber(number_field,{"x":1})
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))

  def test_setString_with_Boolean(self,rtiOutputFixture):
    string_field="color"
    with pytest.raises(AttributeError) as execinfo:
      rtiOutputFixture.instance.setString(string_field,True)
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))

  def test_setString_with_Number(self,rtiOutputFixture):
    string_field="color"
    with pytest.raises(AttributeError) as execinfo:
      rtiOutputFixture.instance.setString(string_field,55.55)
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))
  
  def test_setString_with_Dictionary(self,rtiOutputFixture):
    string_field="color"
    with pytest.raises(AttributeError) as execinfo:
      rtiOutputFixture.instance.setString(string_field,{"color":1})
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))


  def test_setBoolean_with_String(self,rtiOutputFixture):
    boolean_field="z"
    with pytest.raises(TypeError) as execinfo:
      rtiOutputFixture.instance.setBoolean(boolean_field,"str")
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))

  @pytest.mark.xfail
  # Implicit type conversion from number to Boolean 
  def test_setBoolean_with_Number(self,rtiOutputFixture):
    boolean_field="z"
    with pytest.raises(TypeError) as execinfo:
      rtiOutputFixture.instance.setBoolean(boolean_field,55.55)
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))
  
  def test_setBoolean_with_Dictionary(self,rtiOutputFixture):
    boolean_field="z"
    with pytest.raises(TypeError) as execinfo:
      rtiOutputFixture.instance.setBoolean(boolean_field,{"color":1})
    print("\nException of type:"+str(execinfo.type)+ \
      "\nvalue:"+str(execinfo.value))

  # TODO: A dictionary with incompatible types can be set!!!
  @pytest.mark.xfail
  def test_setDictionary_with_incompatible_types(self,rtiOutputFixture,capfd):
    dict_with_incompatible_types={"color":1,"x":"str"}
    rtiOutputFixture.instance.setDictionary(dict_with_incompatible_types)
    out,err = capfd.readouterr()
    assert 0
