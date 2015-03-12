##############################################################################
#    (c) 2005-2015 Copyright, Real-Time Innovations, All rights reserved.    #
#                                                                            #
# RTI grants Licensee a license to use, modify, compile, and create          #
# derivative works of the Software.  Licensee has the right to distribute    #
# object form only for use with RTI products. The Software is provided       #
# "as is", with no warranty of any type, including any warranty for fitness  #
# for any purpose. RTI is under no obligation to maintain or support the     #
# Software.  RTI shall not be liable for any incidental or consequential     #
# damages arising out of the use or inability to use the software.           #
#                                                                            #
##############################################################################

import ctypes
import os
import sys
import weakref
import platform

bits   = platform.machine();
osname = platform.system();
arch = "x64Darwin12clang4.1";
post = "dylib";
libname = 'librti_dds_connector'

path = os.path.dirname(os.path.realpath(__file__))
path = path + "/lib/" + arch + "/";
libname = libname + "." + post
rti = ctypes.CDLL(os.path.join(path, libname), ctypes.RTLD_GLOBAL)

rtin_RTIDDSConnector_new = rti.RTIDDSConnector_new
rtin_RTIDDSConnector_new.restype = ctypes.c_void_p
rtin_RTIDDSConnector_new.argtypes = [ctypes.c_char_p,ctypes.c_char_p]

rtin_RTIDDSConnector_setNumberIntoSamples = rti.RTIDDSConnector_setNumberIntoSamples
rtin_RTIDDSConnector_setNumberIntoSamples.argtypes = [ctypes.c_void_p, ctypes.c_char_p,ctypes.c_char_p,ctypes.c_double]
rtin_RTIDDSConnector_setBooleanIntoSamples = rti.RTIDDSConnector_setBooleanIntoSamples
rtin_RTIDDSConnector_setBooleanIntoSamples.argtypes = [ctypes.c_void_p, ctypes.c_char_p,ctypes.c_char_p,ctypes.c_int]
rtin_RTIDDSConnector_setStringIntoSamples = rti.RTIDDSConnector_setStringIntoSamples
rtin_RTIDDSConnector_setStringIntoSamples.argtypes = [ctypes.c_void_p, ctypes.c_char_p,ctypes.c_char_p,ctypes.c_char_p]

rtin_RTIDDSConnector_write = rti.RTIDDSConnector_write
rtin_RTIDDSConnector_write.argtypes = [ctypes.c_void_p, ctypes.c_char_p]

rtin_RTIDDSConnector_read = rti.RTIDDSConnector_read
rtin_RTIDDSConnector_read.argtypes = [ctypes.c_void_p, ctypes.c_char_p]
rtin_RTIDDSConnector_take = rti.RTIDDSConnector_take
rtin_RTIDDSConnector_take.argtypes = [ctypes.c_void_p, ctypes.c_char_p]

rtin_RTIDDSConnector_getInfosLength = rti.RTIDDSConnector_getInfosLength
rtin_RTIDDSConnector_getInfosLength.restype = ctypes.c_double
rtin_RTIDDSConnector_getInfosLength.argtypes = [ctypes.c_void_p,ctypes.c_char_p]

rtin_RTIDDSConnector_getBooleanFromInfos = rti.RTIDDSConnector_getBooleanFromInfos
rtin_RTIDDSConnector_getBooleanFromInfos.restype  = ctypes.c_int
rtin_RTIDDSConnector_getBooleanFromInfos.argtypes = [ctypes.c_void_p, ctypes.c_char_p, ctypes.c_int, ctypes.c_char_p]

rtin_RTIDDSConnector_getSamplesLength = rti.RTIDDSConnector_getInfosLength
rtin_RTIDDSConnector_getSamplesLength.restype = ctypes.c_double
rtin_RTIDDSConnector_getSamplesLength.argtypes = [ctypes.c_void_p,ctypes.c_char_p]

rtin_RTIDDSConnector_getNumberFromSamples = rti.RTIDDSConnector_getNumberFromSamples
rtin_RTIDDSConnector_getNumberFromSamples.restype = ctypes.c_double
rtin_RTIDDSConnector_getNumberFromSamples.argtypes = [ctypes.c_void_p, ctypes.c_char_p, ctypes.c_int, ctypes.c_char_p]

rtin_RTIDDSConnector_getBooleanFromSamples = rti.RTIDDSConnector_getBooleanFromSamples
rtin_RTIDDSConnector_getBooleanFromSamples.restype = ctypes.c_int
rtin_RTIDDSConnector_getBooleanFromSamples.argtypes = [ctypes.c_void_p, ctypes.c_char_p, ctypes.c_int, ctypes.c_char_p]

rtin_RTIDDSConnector_getStringFromSamples = rti.RTIDDSConnector_getStringFromSamples
rtin_RTIDDSConnector_getStringFromSamples.restype = ctypes.c_char_p
rtin_RTIDDSConnector_getStringFromSamples.argtypes = [ctypes.c_void_p, ctypes.c_char_p, ctypes.c_int, ctypes.c_char_p]

#Python Class Definition

class Samples:
	def __init__(self,input):
		self.input = input;

	def getLength(self):
		return int(rtin_RTIDDSConnector_getSamplesLength(self.input.connector.native,self.input.name));

	def getNumber(self, index, fieldName):
		return rtin_RTIDDSConnector_getNumberFromSamples(self.input.connector.native,self.input.name,index,fieldName);

	def getBoolean(self, index, fieldName):
		return rtin_RTIDDSConnector_getBooleanFromSamples(self.input.connector.native,self.input.name,index,fieldName);

	def getString(self, index, fieldName):
		return rtin_RTIDDSConnector_getStringFromSamples(self.input.connector.native,self.input.name,index,fieldName);


class Infos:
	def __init__(self,input):
		self.input = input;

	def getLength(self):
		return int(rtin_RTIDDSConnector_getInfosLength(self.input.connector.native,self.input.name));

	def isValid(self, index):
		return rtin_RTIDDSConnector_getBooleanFromInfos(self.input.connector.native,self.input.name,index,'valid_data');

class Input:
	def __init__(self, connector, name):
		self.connector = connector;
		self.name = name;
		self.samples = Samples(self);
		self.infos = Infos(self);

	def read(self):
		rtin_RTIDDSConnector_read(self.connector.native,self.name);

	def take(self):
		rtin_RTIDDSConnector_take(self.connector.native,self.name);

class Instance:
	def __init__(self, output):
		self.output = output;

	def setNumber(self, fieldName, value):
		rtin_RTIDDSConnector_setNumberIntoSamples(self.output.connector.native,self.output.name,fieldName,value);

	def setBoolean(self,fieldName, value):
		rtin_RTIDDSConnector_setBooleanIntoSamples(self.output.connector.native,self.output.name,fieldName,value);
	
	def setString(self, fieldName, value):
		rtin_RTIDDSConnector_setStringIntoSamples(self.output.connector.native,self.output.name,fieldName,value);

class Output:
	def __init__(self, connector, name):
		self.connector = connector;
		self.name = name;
		self.instance = Instance(self);

	def write(self):
		return rtin_RTIDDSConnector_write(self.connector.native,self.name);

class Connector:
	def __init__(self, configName, fileName):
		self.native = rtin_RTIDDSConnector_new(configName, fileName);

	def getOutput(self, outputName):
		return Output(self,outputName);

	def getInput(self, inputName):
		return Input(self, inputName);
