filepath = $(cd "$(dirname)"; pwd)
echo $filepath
python convertExcel.py
echo shell is finished!
