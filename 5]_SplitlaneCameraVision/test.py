tolerance = 30
cur_col =   [100, 125, 5]
detected = 0

orange =    [0, 100, 255]
hot_green = [100, 255, 0]
purple =    [255, 0, 100]
hot_pink =  [90, 0, 200] 
yellow =    [0, 255, 255]
pink =      [255, 0, 255] # maybe fix
aqua =      [100, 150, 0] # rename?
blue =      [200, 200, 0] 
lime =      [90, 250, 100]
salmon =    [90, 90, 250]
col_list = [orange, hot_green, purple, hot_pink, yellow, pink, aqua, blue, lime, salmon]

print(f"colour r: {cur_col[2]}")

for col in col_list:
    n = 0
    print(f"iterating {col}")
    lower_b = col[0]-tolerance
    if lower_b < 0:
        lower_b = 0
    upper_b = col[0]+tolerance
    if upper_b > 255:
        upper_b = 255
    lower_g = col[1]-tolerance
    if lower_g < 0:
        lower_g = 0
    upper_g = col[1]+tolerance
    if upper_g > 255:
        upper_g = 255  
    lower_r = col[2]-tolerance
    if lower_r < 0:
        lower_r = 0
    upper_r = col[2]+tolerance
    if upper_r > 255:
        upper_r = 255

    if (cur_col[0] >= lower_b) and (cur_col[0] <= upper_b):
        n = n + 1
        print("b_level correct")
    if(cur_col[1] >= lower_g) and (cur_col[1] <= upper_g):
        n = n + 1
        print("g_level correct")
    if(cur_col[2] >= lower_r) and (cur_col[2] <= upper_r):
        n = n + 1
        print("r_level correct")
    if n == 3:
        print(f"col was {col}, colour {detected}")
        break
    detected = detected + 1