from distutils.core import setup

pkg_name = 'rticonnextdds_connector'

setup(
    name = pkg_name,
    packages = [pkg_name],
    package_data = {pkg_name: ['lib/armv6vfphLinux3.xgcc4.7.2/*',
                               'lib/armv7aAndroid2.3gcc4.8/*',
                               'lib/armv7aQNX6.5.0SP1qcc_cpp4.4.2/*',
                               'lib/i86Linux3.xgcc4.6.3/*',
                               'lib/i86Win32VS2010/*',
                               'lib/x64Darwin12clang4.1/*',
                               'lib/x64Linux2.6gcc4.4.5/*']}
)
