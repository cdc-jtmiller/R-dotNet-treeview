> install.packages("rcompanion")
> install.packages("FSA")
> install.packages("psych")

> setwd("C:/Work/R_projects/libraries")
> Sodium <- read.csv("C:/Work/VS_projects/Projects/R_treeview_ex/Supplemental/Sodium.csv")
> View(Sodium)

> Sodium$Diet = factor(Sodium$Diet, levels=unique(Sodium$Diet))

> install.packages("psych")
> library(psych)
> headTail(Sodium)
> str(Sodium)
> summary(Sodium)

> Summarize(Sodium ~ Diet, data=Sodium, digits=3)
> boxplot(Sodium ~ Diet, data = Sodium)
> library(rcompanion)
