# ====================================================================
# 検証実行スクリプト
# ====================================================================
# 変数定義
$invokeTime = Get-Date
$outputDate = $invokeTime.ToString("yyyyMMdd")

Write-Host 出力ファイル：bin/Result/$outputDate/cobertura.xml

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=bin/Result/$outputDate/cobertura.xml
if ($?) {
	# 検証成功
	reportgenerator -reports:bin/Result/$outputDate/cobertura.xml -targetdir:bin/Result/$outputDate -reporttypes:Html
	if ($?) {
		# 生成成功
		Start-Process bin/Result/$outputDate/index.html
	}
}
