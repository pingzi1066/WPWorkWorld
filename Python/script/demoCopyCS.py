#coding:utf-8

import shutil
import os
import re
import kmTools
import sys

if sys.version_info < (3,4):
	reload(sys)
	sys.setdefaultencoding('utf-8')
	pass

#取当前文件所在目录
filePath = os.path.split(os.path.realpath(__file__))[0]

defaultCS = filePath+"/default/defaultCS.cs"

#默认目录
testPath = filePath + "/default/"

#输出目录
testOutPath = filePath + "/testCopy/"

print("--------------" + filePath)

#从根目录获得所有的分目录、文件名。root 为地址，dirs 为地址下的文件夹 、 files为地址下的文件
for root, dirs, files in os.walk(testPath):
    for f in files:
        #取目录+文件名，f只是文件名
        filePath = os.path.join(root, f)
        
        #fname 为文件名， fextension 为后缀
        fname, fextension = os.path.splitext(f)
        
        flieHeadName = fname[0:1].upper() + fname[1:]
        
        #打印第一个字母大字     #打印第一个之后的文件名 fname[1:]
        print(flieHeadName + "----------" + f)
        
        if fextension==".cs":
            print unicode('是CS 脚本','utf-8').encode('gbk')
            print "是CS 脚本"

        #声明文件 并命名
        outFile = os.path.join(testOutPath,"Static" + flieHeadName + ".cs")
        
        #注明一个打开的文件
        fp = kmTools.openFile(filePath,'r')
        
        #读出文件每一行
        allLine = fp.readlines()

        fp.close();
#fp = kmTools.openFile(outFile,'w')
        
        #开始写文件
        #for eachLine in allLine
            #写入
#a = re.sub('defaultDataManageclass',outFile)
        #end
