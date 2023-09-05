#!/bin/sh
cd `dirname $0`
while true
do
    ./DcamStandalone.app/Contents/MacOS/DcamStandalone && exit -1
done
