## sets working dir
#  setwd("C:\\Work\\VS_projects\\Projects\\R_treeview_ex\\User_library\\")

## runs file from command line
#  source("paired_ttest.r",echo=TRUE)

## Exports data from .Net program
#  write.csv(dataset, "C:/Work/VS_projects/Projects/R_treeview_ex/Supplemental/testing.csv", quote=F, row.names=F)

## Sets seed for repetitive number generation
set.seed(2820)

## Creates the matrices
preTest <- c(rnorm(100, mean = 145, sd = 9))
postTest <- c(rnorm(100, mean = 138, sd = 8))
ID <- c(1:100)

## Runs paired-sample T-Test just on two original matrices
t.test(preTest,postTest, paired = TRUE)

## Converts the matrices into a dataframe that looks like the way these data are normally stored
pstt <- data.frame(ID,preTest,postTest)
t.test(pstt$preTest,pstt$postTest, paired = TRUE)
#print(pstt)

## Puts the data in a form that can be used by R (grouping var | data var)
pstt2 <- data.frame(
				group = rep(c("preTest","postTest"),each = 100),
				weight = c(preTest, postTest)
				)				
				
#print(pstt2)				

## Runs paired-sample T-Test just on two original matrices
#  t.test(preTest,postTest, paired = TRUE)

## Runs paired-sample T-Test on the newly structured data frame
#  Must be sure to use 'relevel() to set which reference category in 'group' comes first
t.test(weight ~ relevel(group, "preTest"), data = pstt2, paired = TRUE)

