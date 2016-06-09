import pytest
class TestOutput:

  # TODO: Output object should not get created if DW name does not exist in XML
  """
  All methods: setNumber, setBoolean,  setString, setDictionary on Instance and 
  write fail
  """
  def test_invalid_DW(self,connector):
    invalid_DW = "InvalidDW"
    with pytest.raises(ValueError):
      op= connector.getOutput(invalid_DW)
    
  # TODO: No exception is thrown when a non-existent field is accessed
  def test_setNumber_on_nonexistant_field(self,out,capfd):
    non_existant_field="invalid_field"
    out.instance.setNumber(non_existant_field,1)
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  def test_setString_on_nonexistant_field(self,out,capfd):
    non_existant_field="invalid_field"
    out.instance.setString(non_existant_field,"1")
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  def test_setBoolean_on_nonexistant_field(self,out,capfd):
    non_existant_field="invalid_field"
    out.instance.setBoolean(non_existant_field,True)
    out,err = capfd.readouterr()
    assert "RTILuaDynamicData_set:!get kind failed" in out

  def test_setDictionary_with_nonexistant_fields(self,out,capfd):
    invalid_dictionary= {"non_existant_field":"value"}
    out.instance.setDictionary(invalid_dictionary)
    out,err = capfd.readouterr()
    assert "RTILuaJsonHelper_parse_json_node:!get kind failed" in out

  # TODO: Should user be notified/warned about type incompatibility
  @pytest.mark.xfail
  def test_setNumber_with_Boolean_value(self,out):
    number_field="x"
    with pytest.raises(Exception) as execinfo:
      out.instance.setNumber(number_field,True)
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("Traceback:"+str(execinfo.traceback))

  # TODO: ctypes exception is not propagated via the connector
  def test_setNumber_with_String_value(self,out):
    number_field="x"
    with pytest.raises(Exception) as execinfo:
      out.instance.setNumber(number_field,"str")
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("\nTraceback:"+str(execinfo.traceback))
 

  def test_setString_with_Boolean_value(self,out):
    string_field="color"
    with pytest.raises(Exception) as execinfo:
      out.instance.setString(string_field,True)
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("Traceback:"+str(execinfo.traceback))

  def test_setString_with_Number_value(self,out):
    string_field="color"
    with pytest.raises(Exception) as execinfo:
      out.instance.setString(string_field,55.55)
    print("\nException of type:"+str(execinfo.type)+ "\nvalue:"+str(execinfo.value))
    print("\nTraceback:"+str(execinfo.traceback))
  
  # TODO: implement tests for setBoolean for types String,Number

  # TODO: A dictionary with incompatible types can be set!!!
  def test_setDictionary_with_incompatible_types(self,out,capfd):
    dict_with_incompatible_types={"color":1,"x":"str"}
    out.instance.setDictionary(dict_with_incompatible_types)
    out,err = capfd.readouterr()
    assert 0
