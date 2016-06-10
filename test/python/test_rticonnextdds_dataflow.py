import pytest,time,sys,os
sys.path.append(os.path.dirname(os.path.realpath(__file__)) + "/../../")
import rticonnextdds_connector as rti

class TestRead:
  @pytest.fixture(scope="class")
  def testMsg(self):
    return {"x":1,"y":1,"z":True,"color":"BLUE","shapesize":5}

  @pytest.fixture(autouse=True,params=["read","take"])
  def sendTestMsg(self,request,rtiInputFixture,rtiOutputFixture,testMsg):
    # take any pre-existing samples from cache
    rtiInputFixture.take()
    #test_msg={"x":1,"y":1,"z":True,"color":"BLUE","shapesize":5}
    rtiOutputFixture.instance.setDictionary(testMsg)
    rtiOutputFixture.write()
    for i in range(1,20):
        time.sleep(.5)
        retrieve_func= getattr(rtiInputFixture,request.param)
        retrieve_func() 
        if rtiInputFixture.samples.getLength() > 0:
          break

  def test_samples_getLength(self,rtiInputFixture):
    assert rtiInputFixture.samples.getLength() == 1 

  def test_infos_getLength(self,rtiInputFixture):
    assert rtiInputFixture.infos.getLength() == 1 

  def test_infos_isValid(self,rtiInputFixture):
    assert rtiInputFixture.infos.isValid(1)== True

  def test_getDictionary(self,rtiInputFixture,testMsg):
    received_msg = rtiInputFixture.samples.getDictionary(1)
    assert received_msg==testMsg

  def test_getTypes(self,rtiInputFixture,testMsg):
    x = rtiInputFixture.samples.getNumber(1,"x")
    y = rtiInputFixture.samples.getNumber(1,"y")
    z = rtiInputFixture.samples.getBoolean(1,"z")
    color  = rtiInputFixture.samples.getString(1,"color")
    shapesize = rtiInputFixture.samples.getNumber(1,"shapesize")
    assert x == testMsg['x'] and y == testMsg['y'] \
      and z == testMsg['z'] and color == testMsg['color'] \
      and shapesize == testMsg['shapesize']

  ## TODO: accessing non-existent field should throw an exception
  def test_getNumber_for_nonexistent_field(self,capfd,rtiInputFixture,testMsg):
    x = rtiInputFixture.samples.getNumber(1,"invalid_field")
    out,err=capfd.readouterr()
    assert "DynamicData_get:!get kind failed" in out

  def test_getString_for_nonexistent_field(self,capfd,rtiInputFixture,testMsg):
    x = rtiInputFixture.samples.getString(1,"invalid_field")
    out,err=capfd.readouterr()
    assert "DynamicData_get:!get kind failed" in out

  def test_getBoolean_for_nonexistent_field(self,capfd,rtiInputFixture,testMsg):
    x = rtiInputFixture.samples.getBoolean(1,"invalid_field")
    out,err=capfd.readouterr()
    assert "DynamicData_get:!get kind failed" in out

  # TODO: Test invalid type access for getNumber,getString & getBoolean
  """
  getString on numeric field gives a string representation of the number. 
  getString on boolean field gives None 
 
  getBoolean on string or numeric field gives an int with value of 0/1
  getBoolean on Boolean returns int value of 0/1
  
  getNumber on a boolean field gives a float value = 0.0
  getNumber on string field gives a float = 0.0 
  """
