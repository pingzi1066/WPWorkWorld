#coding:utf-8

import os
rootPath = os.path.split(os.path.realpath(__file__))[0]

# 工具脚本所在目录
import script.convertExcel
convert = script.convertExcel

mainPath = rootPath + "/excel2json"

excelPath = mainPath + "/table/" #表格位置
exportPath = mainPath + "/json/" #导出的所有的json目录
recordPath = mainPath + "/record/" #文件导出记录
exportNewPath = mainPath + "/new/" #当前导出已经改变过的json目录

convert.convert(excelPath,exportPath,recordPath,exportNewPath)