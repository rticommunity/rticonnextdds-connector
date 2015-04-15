{
  'variables': {
    'app_name': 'RTI Connext Connector',
    'system': '<!(uname -s)',
    'release': '',
    'rtiLibrary': 'rtiddsprototyper',
    'conditions': [
      ['target_arch=="ia32"', {
        'target_arch': 'i86'
      }],
      ['OS!="win"', {
        'system%': '<!(uname -s)',
        'release%': '<!(uname -r)'
      }],
      ['OS=="mac"', {
      	'libDir%' : "<(module_root_dir)/bin/<(target_arch)<(system)12clang4.1",
        'rtiLib%': "librti_dds_connector.dylib"
      }],
    ]
  },
  'targets': [{
    'target_name': 'rtiddsconnector',
    'sources': [
      'src/rtiddsconnector.cc',
      'src/connector.cc'
    ],
    "cflags_cc!": [ "-fno-rtti", "-fno-exceptions" ],
    "cflags!": [ "-fno-exceptions" ],
    "include_dirs" : [
      "<!(node -e \"require('nan')\")"
    ],
    'conditions': [
      ['OS=="mac"', {
        "xcode_settings": {
          'OTHER_CPLUSPLUSFLAGS' : ['-std=c++11','-stdlib=libc++', '-v'],
          'OTHER_LDFLAGS': ['-stdlib=libc++'],
          'MACOSX_DEPLOYMENT_TARGET': '10.7',
          'GCC_ENABLE_CPP_EXCEPTIONS': 'YES'
        },
        'link_settings': {
          'libraries': [
            '-lrti_dds_connector'
          ],
          'library_dirs': [
            '<(libDir)',
          ],
        },
      }]
    ]
  }]
}
