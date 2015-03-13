@echo off
del HelloWorld.class
del HelloWorld.nxj
nxjc HelloWorld.java
nxjlink nxjlink -v HelloWorld -o HelloWorld.nxj
nxjupload HelloWorld.nxj