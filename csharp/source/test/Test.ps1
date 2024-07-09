# ====================================================================
# 検証実行スクリプト
# ====================================================================
# 変数定義
$invokeTime = Get-Date
$outputDate = $invokeTime.ToString("yyyyMMdd")
$outputPath = "bin/Result/$outputDate"
$reportFile = "$outputPath/cobertura.xml"

Write-Host 出力ファイル：$reportFile

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=$reportFile
if ($?) {
	# 検証成功
	reportgenerator -reports:$reportFile -targetdir:$outputPath -reporttypes:Html
	if ($?) {
		# 生成成功
		Start-Process $outputPath/index.html
	}
}
