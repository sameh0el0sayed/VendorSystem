@ECHO OFF
ECHO Create our website....
SET HOME_DIR=%HOMEDRIVE%\qbs-website
if not exist %HOME_DIR% (
    mkdir %HOME_DIR%   
)